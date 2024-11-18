// Copyright 2024 Mindbox Ltd
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mindbox.YandexTracker.JsonConverters;

namespace Mindbox.YandexTracker;

/// <inheritdoc />
public sealed class YandexTrackerClient : IYandexTrackerClient
{
	private const string TotalPageHeaderName = "X-Total-Pages";

	/// <summary>
	/// Представляет настройки пагинации по умолчанию, которые применяются в случаях,
	/// когда параметры пагинации не были переданы в текущем запросе и не указаны в конфигурации.
	/// </summary>
	public static readonly PaginationSettings DefaultPaginationSettings = new();

	private readonly IOptionsMonitor<YandexTrackerClientOptions> _options;
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _jsonOptions;

	public YandexTrackerClient(
		IOptionsMonitor<YandexTrackerClientOptions> options,
		HttpClient httpClient)
	{
		ArgumentNullException.ThrowIfNull(options);
		ArgumentNullException.ThrowIfNull(httpClient);

		_options = options;
		_httpClient = httpClient;
		ConfigureHttpClient(_httpClient, _options.CurrentValue);

		_jsonOptions = new JsonSerializerOptions
		{
			Converters =
			{
				// у этиъ 2 enum'ов не camelCase, а какая-то своя фигня в Трекере
				// (например, "ru.yandex.startrek.core.fields.UserFieldType" или snake_case)
				new EnumWithEnumMemberAttributeJsonConverter<QueueLocalFieldType>(),
				new EnumWithEnumMemberAttributeJsonConverter<ProjectEntityStatus>(),
				// а остальные enum'ы - camelCase
				new JsonStringEnumConverter(namingPolicy: JsonNamingPolicy.CamelCase),
				new YandexDateTimeJsonConverter(),
				new YandexNullableDateTimeJsonConverter()
			},
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};
	}

	private static void ConfigureHttpClient(
		HttpClient httpClient,
		YandexTrackerClientOptions options)
	{
		httpClient.DefaultRequestHeaders.UserAgent.Add(
			new ProductInfoHeaderValue(
				"Mindbox.YandexTrackerClient",
				Assembly.GetExecutingAssembly().GetName().Version!.ToString()));
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", options.OAuthToken);
		httpClient.DefaultRequestHeaders.Add("X-Cloud-Org-ID", options.Organization);
		if (options.LanguageTag is not null)
			httpClient.DefaultRequestHeaders.Add("Accept-Language", options.LanguageTag);
	}

	/// <inheritdoc />
	public Task<GetQueuesResponse> GetQueueAsync(
		string queueKey,
		QueueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(queueKey);

		Dictionary<string, string>? parameters = null;

		if (expand is not null and not QueueExpandData.None)
		{
			parameters = new Dictionary<string, string>
			{
				["expand"] = expand.Value.ToYandexQueryString(QueueExpandData.All, QueueExpandData.None)
			};
		}

		return ExecuteYandexTrackerApiRequestAsync<GetQueuesResponse>(
			$"queues/{queueKey}",
			HttpMethod.Get,
			payload: null,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetQueuesResponse>> GetQueuesAsync(
		QueuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		Dictionary<string, string>? parameters = null;

		if (expand is not null && (QueuesExpandData)expand != QueuesExpandData.None)
		{
			parameters = new Dictionary<string, string>
			{
				["expand"] = expand.Value.ToYandexQueryString(null, QueuesExpandData.None)
			};
		}

		return ExecuteYandexTrackerCollectionRequestAsync<GetQueuesResponse>(
			"queues",
			HttpMethod.Get,
			parameters: parameters,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<GetIssueResponse> GetIssueAsync(
		string issueKey,
		IssueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(issueKey);

		Dictionary<string, string>? parameters = null;

		if (expand is not null and not IssueExpandData.None)
		{
			parameters = new Dictionary<string, string>
			{
				["expand"] = expand.Value.ToYandexQueryString(null, IssueExpandData.None)
			};
		}

		return ExecuteYandexTrackerApiRequestAsync<GetIssueResponse>(
			$"issues/{issueKey}",
			HttpMethod.Get,
			payload: null!,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueResponse>> GetIssuesFromQueueAsync(
		string queueKey,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(queueKey);

		Dictionary<string, string>? parameters = null;

		if (expand is not null and not IssuesExpandData.None)
		{
			parameters = new Dictionary<string, string>
			{
				["expand"] = expand.Value.ToYandexQueryString(null, IssuesExpandData.None)
			};
		}

		var request = new GetIssuesFromQueueRequest
		{
			Queue = queueKey
		};

		return ExecuteYandexTrackerCollectionRequestAsync<GetIssueResponse>(
			"issues/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueResponse>> GetIssuesByKeysAsync(
		IReadOnlyList<string> issueKeys,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(issueKeys);

		Dictionary<string, string>? parameters = null;

		if (expand is not null and not IssuesExpandData.None)
		{
			parameters = new Dictionary<string, string>
			{
				["expand"] = expand.Value.ToYandexQueryString(null, IssuesExpandData.None)
			};
		}

		var request = new GetIssuesFromKeysRequest
		{
			Keys = issueKeys
		};

		return ExecuteYandexTrackerCollectionRequestAsync<GetIssueResponse>(
			"issues/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueResponse>> GetIssuesByFilterAsync(
		GetIssuesByFilterRequest request,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		Dictionary<string, string>? parameters = null;

		if (expand is not null and not IssuesExpandData.None)
		{
			parameters = new Dictionary<string, string>
			{
				["expand"] = expand.Value.ToYandexQueryString(null, IssuesExpandData.None)
			};
		}

		return ExecuteYandexTrackerCollectionRequestAsync<GetIssueResponse>(
			"issues/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueResponse>> GetIssuesByQueryAsync(
		string query,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(query);

		Dictionary<string, string>? parameters = null;

		if (expand is not null and not IssuesExpandData.None)
		{
			parameters = new Dictionary<string, string>
			{
				["expand"] = expand.Value.ToYandexQueryString(null, IssuesExpandData.None)
			};
		}

		var request = new GetIssuesByQueryRequest
		{
			Query = query
		};

		return ExecuteYandexTrackerCollectionRequestAsync<GetIssueResponse>(
			"issues/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	public async Task<YandexTrackerCollectionResponse<GetIssueChangelogResponse>> GetIssueChangelogAsync(
		string issueKey,
		string? fieldKey = null,
		IssueChangeType? type = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(issueKey);

		var parameters = new Dictionary<string, string>();

		if (fieldKey is not null)
			parameters["field"] = fieldKey;
		if (type is not null)
			parameters["type"] = type.Value.ToCamelCase();

		return await ExecuteYandexTrackerCollectionRequestAsync<GetIssueChangelogResponse>(
			$"issues/{issueKey}/changelog",
			HttpMethod.Get,
			parameters: parameters,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<CreateIssueResponse> CreateIssueAsync(
		CreateIssueRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		return ExecuteYandexTrackerApiRequestAsync<CreateIssueResponse>(
			"issues",
			HttpMethod.Post,
			payload: request,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<ImportIssueResponse> ImportIssueAsync(ImportIssueRequest request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		return ExecuteYandexTrackerApiRequestAsync<ImportIssueResponse>(
			"issues/_import",
			HttpMethod.Post,
			payload: request,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetComponentResponse>> GetComponentsAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<GetComponentResponse>(
			"components",
			HttpMethod.Get,
			payload: null!,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<CreateComponentResponse> CreateComponentAsync(
		CreateComponentRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		return ExecuteYandexTrackerApiRequestAsync<CreateComponentResponse>(
			"components",
			HttpMethod.Post,
			payload: request,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetCommentsResponse>> GetCommentsAsync(
		string issueKey,
		CommentExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(issueKey);

		Dictionary<string, string>? parameters = null;

		if (expand is not null and not CommentExpandData.None)
		{
			parameters = new Dictionary<string, string>
			{
				["expand"] = expand.Value.ToYandexQueryString(CommentExpandData.All, CommentExpandData.None)
			};
		}

		return ExecuteYandexTrackerCollectionRequestAsync<GetCommentsResponse>(
			$"issues/{issueKey}/comments",
			HttpMethod.Get,
			parameters: parameters,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<CreateCommentResponse> CreateCommentAsync(
		string issueKey,
		CreateCommentRequest request,
		bool? addAuthorToFollowers = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(issueKey);
		ArgumentNullException.ThrowIfNull(request);

		Dictionary<string, string>? parameters = null;

		if (addAuthorToFollowers.HasValue)
		{
			parameters = new Dictionary<string, string>
			{
				["isAddToFollowers"] = addAuthorToFollowers.Value.ToString()
			};
		}

		return ExecuteYandexTrackerApiRequestAsync<CreateCommentResponse>(
			$"issues/{issueKey}/comments",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<ImportCommentResponse> ImportCommentAsync(
		string issueKey,
		ImportCommentRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(issueKey);
		ArgumentNullException.ThrowIfNull(request);

		return ExecuteYandexTrackerApiRequestAsync<ImportCommentResponse>(
			$"issues/{issueKey}/comments/_import",
			HttpMethod.Post,
			payload: request,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public async Task<YandexTrackerCollectionResponse<GetAttachmentResponse>> GetAttachmentsAsync(
		string issueKey,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(issueKey);

		return await ExecuteYandexTrackerCollectionRequestAsync<GetAttachmentResponse>(
			$"issues/{issueKey}/attachments",
			HttpMethod.Get,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public async Task<CreateAttachmentResponse> CreateAttachmentAsync(
		string issueKey,
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(issueKey);
		ArgumentNullException.ThrowIfNull(fileStream);

		Dictionary<string, string>? parameters = null;

		if (newFileName is not null)
		{
			parameters = new Dictionary<string, string>
			{
				["filename"] = newFileName
			};
		}

		using var form = new MultipartFormDataContent();
		using var fileContent = new StreamContent(fileStream);
		form.Add(fileContent, "file", newFileName ?? "file");

		return await ExecuteYandexTrackerApiRequestAsync<CreateAttachmentResponse>(
			$"issues/{issueKey}/attachments",
			HttpMethod.Post,
			payload: form,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	public async Task<ImportAttachmentResponse> ImportAttachmentToIssueAsync(
		string issueKey,
		Stream fileStream,
		string newFileName,
		DateTime createdAt,
		string createdBy,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(issueKey);
		ArgumentNullException.ThrowIfNull(fileStream);
		ArgumentNullException.ThrowIfNull(newFileName);
		ArgumentNullException.ThrowIfNull(createdBy);

		var parameters = new Dictionary<string, string>
		{
			["filename"] = newFileName,
			["createdAt"] = createdAt.ToString(YandexTrackerConstants.DateTimeFormat, CultureInfo.InvariantCulture),
			["createdBy"] = createdBy,
		};

		using var form = new MultipartFormDataContent();
		using var fileContent = new StreamContent(fileStream);
		form.Add(fileContent, "file", newFileName);

		return await ExecuteYandexTrackerApiRequestAsync<ImportAttachmentResponse>(
			$"issues/{issueKey}/attachments/_import",
			HttpMethod.Post,
			payload: form,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	public async Task<ImportAttachmentResponse> ImportAttachmentToIssueCommentAsync(
		string issueKey,
		string commentId,
		Stream fileStream,
		string newFileName,
		DateTime createdAt,
		string createdBy,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(issueKey);
		ArgumentNullException.ThrowIfNull(commentId);
		ArgumentNullException.ThrowIfNull(fileStream);
		ArgumentNullException.ThrowIfNull(newFileName);
		ArgumentNullException.ThrowIfNull(createdBy);

		var parameters = new Dictionary<string, string>
		{
			["filename"] = newFileName,
			["createdAt"] = createdAt.ToString(YandexTrackerConstants.DateTimeFormat, CultureInfo.InvariantCulture),
			["createdBy"] = createdBy,
		};

		using var form = new MultipartFormDataContent();
		using var fileContent = new StreamContent(fileStream);
		form.Add(fileContent, "file", newFileName);

		return await ExecuteYandexTrackerApiRequestAsync<ImportAttachmentResponse>(
			$"issues/{issueKey}/comments/{commentId}/attachments/_import",
			HttpMethod.Post,
			payload: form,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public async Task<CreateAttachmentResponse> CreateTemporaryAttachmentAsync(
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(fileStream);

		using var form = new MultipartFormDataContent();
		using var fileContent = new StreamContent(fileStream);
		form.Add(fileContent, "file", newFileName ?? "file");

		return await ExecuteYandexTrackerApiRequestAsync<CreateAttachmentResponse>(
			"attachments",
			HttpMethod.Post,
			payload: form,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<string>> GetTagsAsync(
		string queueKey,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(queueKey);

		return ExecuteYandexTrackerCollectionRequestAsync<string>(
			$"queues/{queueKey}/tags",
			HttpMethod.Get,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<CreateProjectResponse> CreateProjectAsync(
		ProjectEntityType entityType,
		CreateProjectRequest request,
		ProjectFieldData? returnedFields = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		Dictionary<string, string>? parameters = null;

		if (returnedFields is not null and not ProjectFieldData.None)
		{
			parameters = new Dictionary<string, string>
			{
				["fields"] = returnedFields.Value.ToYandexQueryString(null, ProjectFieldData.None)
			};
		}

		return ExecuteYandexTrackerApiRequestAsync<CreateProjectResponse>(
			$"entities/{entityType.ToCamelCase()}",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public async Task<YandexTrackerCollectionResponse<ProjectInfo>> GetProjectsAsync(
		ProjectEntityType entityType,
		GetProjectsRequest request,
		ProjectFieldData? returnedFields = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		var parameters = new Dictionary<string, string>();

		if (returnedFields is not null and not ProjectFieldData.None)
		{
			parameters["fields"] = returnedFields.Value.ToYandexQueryString(null, ProjectFieldData.None);
		}

		var pagination = paginationSettings ?? _options.CurrentValue.DefaultPaginationSettings ?? DefaultPaginationSettings;

		var page = 1;
		parameters["page"] = page.ToString(CultureInfo.InvariantCulture);
		parameters["perPage"] = pagination.PerPage.ToString(CultureInfo.InvariantCulture);

		var projects = new List<ProjectInfo>();

		// При запросе проектов возвращается респонс
		// {
		// "pages": pageCount,
		// "hits": кол-во проектов в values,
		// "values": [{}]
		// },
		// Логика в обычной пагинации не подойдет, приходится обрабатывать этот случай отдельно
		GetProjectsResponse? response;
		do
		{
			response = await ExecuteYandexTrackerApiRequestAsync<GetProjectsResponse>(
				$"entities/{entityType.ToCamelCase()}/_search",
				HttpMethod.Post,
				payload: request,
				parameters: parameters,
				cancellationToken: cancellationToken);

			projects.AddRange(response.Values);
			page++;
			parameters["page"] = page.ToString(CultureInfo.InvariantCulture);

		} while (
			response.Pages > page
			&& (pagination.MaxPageRequestCount is null || page < pagination.MaxPageRequestCount));

		return new YandexTrackerCollectionResponse<ProjectInfo>
		{
			FetchedPages = page,
			TotalPages = response.Pages,
			Values = projects
		};
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueFieldsResponse>> GetLocalQueueFieldsAsync(
		string queueKey,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(queueKey);

		return ExecuteYandexTrackerCollectionRequestAsync<GetIssueFieldsResponse>(
			$"queues/{queueKey}/localFields",
			HttpMethod.Get,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueFieldsResponse>> GetGlobalFieldsAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<GetIssueFieldsResponse>(
			"fields",
			HttpMethod.Get,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<UserDetailedInfoDto> GetMyselfAsync(CancellationToken cancellationToken)
	{
		return ExecuteYandexTrackerApiRequestAsync<UserDetailedInfoDto>(
			"myself",
			HttpMethod.Get,
			payload: null,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<UserDetailedInfoDto> GetUserByIdAsync(
		string userId,
		CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerApiRequestAsync<UserDetailedInfoDto>(
			$"users/{userId}",
			HttpMethod.Get,
			payload: null,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<UserDetailedInfoDto>> GetUsersAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<UserDetailedInfoDto>(
			"users",
			HttpMethod.Get,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueTypeResponse>> GetIssueTypesAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<GetIssueTypeResponse>(
			"issuetypes",
			HttpMethod.Get,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetResolutionResponse>> GetResolutionsAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<GetResolutionResponse>(
			"resolutions",
			HttpMethod.Get,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueStatusResponse>> GetIssueStatusesAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<GetIssueStatusResponse>(
			"statuses",
			HttpMethod.Get,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<CreateQueueLocalFieldResponse> CreateLocalFieldInQueueAsync(
		string queueKey,
		CreateQueueLocalFieldRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(queueKey);
		ArgumentNullException.ThrowIfNull(request);

		return ExecuteYandexTrackerApiRequestAsync<CreateQueueLocalFieldResponse>(
			$"queues/{queueKey}/localFields",
			HttpMethod.Post,
			payload: request,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetFieldCategoriesResponse>> GetFieldCategoriesAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<GetFieldCategoriesResponse>(
			"fields/categories",
			HttpMethod.Get,
			customPaginationSettings: paginationSettings,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task<CreateQueueResponse> CreateQueueAsync(
		CreateQueueRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		return ExecuteYandexTrackerApiRequestAsync<CreateQueueResponse>(
			"queues",
			HttpMethod.Post,
			payload: request,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteQueueAsync(string queueKey, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(queueKey);

		return ExecuteYandexTrackerApiRequestAsync(
			$"queues/{queueKey}",
			HttpMethod.Delete,
			payload: null,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteCommentAsync(
		string issueKey,
		int commentId,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(issueKey);

		return ExecuteYandexTrackerApiRequestAsync(
			$"issues/{issueKey}/comments/{commentId}",
			HttpMethod.Delete,
			payload: null,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteAttachmentAsync(string issueKey, string attachmentKey, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrEmpty(issueKey);
		ArgumentException.ThrowIfNullOrEmpty(attachmentKey);

		return ExecuteYandexTrackerApiRequestAsync(
			$"issues/{issueKey}/attachments/{attachmentKey}",
			HttpMethod.Delete,
			payload: null,
			cancellationToken: cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteProjectAsync(
		ProjectEntityType entityType,
		int projectShortId,
		bool? deleteWithBoard = null,
		CancellationToken cancellationToken = default)
	{
		Dictionary<string, string>? parameters = null;

		if (deleteWithBoard is not null)
		{
			parameters = new Dictionary<string, string>
			{
				["withBoard"] = deleteWithBoard.ToString()!
			};
		}

		return ExecuteYandexTrackerApiRequestAsync(
			$"entities/{entityType.ToCamelCase()}/{projectShortId}",
			HttpMethod.Delete,
			payload: null,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	private async Task<TResult> ExecuteYandexTrackerApiRequestAsync<TResult>(
		string requestTo,
		HttpMethod method,
		object? payload,
		IDictionary<string, string>? parameters = null,
		IDictionary<string, string>? headers = null,
		CancellationToken cancellationToken = default)
		where TResult : class
	{
		var httpResponse = await ExecuteYandexTrackerApiRawRequestAsync(
			requestTo,
			method,
			payload,
			parameters,
			headers,
			cancellationToken);

		var resultContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
		return JsonSerializer.Deserialize<TResult>(resultContent, _jsonOptions)!;
	}

	private async Task<HttpResponseMessage> ExecuteYandexTrackerApiRawRequestAsync(
		string requestTo,
		HttpMethod method,
		object? payload,
		IDictionary<string, string>? parameters = null,
		IDictionary<string, string>? headers = null,
		CancellationToken cancellationToken = default)
	{
		var apiVersion = "v2";

		var effectivePath = $"{apiVersion}/{requestTo}";
		var baseAddress = new Uri("https://api.tracker.yandex.net/");
		var requestUri = new Uri(baseAddress, effectivePath);
		if (parameters is not null)
		{
			requestUri = new Uri(AddQueryString(requestUri.ToString(), parameters!));
		}

		using var request = new HttpRequestMessage(method, requestUri);

		if (payload is not null)
		{
			if (payload is HttpContent content)
			{
				request.Content = content;
			}
			else
			{
				var json = JsonSerializer.Serialize(payload, _jsonOptions);
				request.Content = new StringContent(
					json,
					Encoding.UTF8,
					"application/json");
			}
		}

		if (headers is not null)
		{
			foreach (var header in headers)
			{
				request.Headers.Add(header.Key, header.Value);
			}
		}

		var response = await _httpClient.SendAsync(request, cancellationToken);

		if (response.IsSuccessStatusCode) return response;

		string errorMessage;
		try
		{
			errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
		}
		catch
		{
			errorMessage = "Unknown error";
		}

		throw new YandexTrackerException(
			$"Request was not successful: {response.StatusCode} : {errorMessage}",
			response.StatusCode);
	}

	private async Task<YandexTrackerCollectionResponse<TResult>> ExecuteYandexTrackerCollectionRequestAsync<TResult>(
		string requestTo,
		HttpMethod httpMethod,
		object? payload = null,
		IDictionary<string, string>? parameters = null,
		IDictionary<string, string>? headers = null,
		PaginationSettings? customPaginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var paginationSettings = customPaginationSettings
								 ?? _options.CurrentValue.DefaultPaginationSettings
								 ?? DefaultPaginationSettings;

		var pageNumber = paginationSettings.StartPage - 1;
		var totalPageCount = 1;

		var result = new List<TResult>();
		do
		{
			pageNumber++;
			var parametersWithPaging = parameters is not null
				? new Dictionary<string, string>(parameters)
				: [];

			parametersWithPaging["perPage"] = paginationSettings.PerPage.ToString(CultureInfo.InvariantCulture);
			parametersWithPaging["page"] = pageNumber.ToString(CultureInfo.InvariantCulture);

			var response = await ExecuteYandexTrackerApiRawRequestAsync(
				requestTo,
				httpMethod,
				payload: payload,
				parametersWithPaging,
				headers,
				cancellationToken);

			var resultContent = await response.Content.ReadAsStringAsync(cancellationToken);
			var dataChunk = JsonSerializer.Deserialize<List<TResult>>(resultContent, _jsonOptions)!;

			result.AddRange(dataChunk);

			if (response.Headers.TryGetValues(TotalPageHeaderName, out var headerValue))
			{
				totalPageCount = Convert.ToInt32(headerValue.First(), CultureInfo.InvariantCulture);
			}
		}
		while (
			pageNumber < totalPageCount
			&& (paginationSettings.MaxPageRequestCount is null || pageNumber < paginationSettings.MaxPageRequestCount));

		return new YandexTrackerCollectionResponse<TResult>
		{
			StartPage = paginationSettings.StartPage,
			FetchedPages = pageNumber,
			TotalPages = totalPageCount,
			Values = result
		};
	}

	private async Task ExecuteYandexTrackerApiRequestAsync(
		string requestTo,
		HttpMethod method,
		object? payload,
		IDictionary<string, string>? parameters = null,
		IDictionary<string, string>? headers = null,
		CancellationToken cancellationToken = default)
	{
		_ = await ExecuteYandexTrackerApiRawRequestAsync(
			requestTo,
			method,
			payload,
			parameters,
			headers,
			cancellationToken);
	}

	private static string AddQueryString(
		string uri,
		IEnumerable<KeyValuePair<string, string?>> queryString)
	{
		ArgumentNullException.ThrowIfNull(uri);
		ArgumentNullException.ThrowIfNull(queryString);

		var anchorIndex = uri.IndexOf('#', StringComparison.Ordinal);
		var uriToBeAppended = uri.AsSpan();
		var anchorText = ReadOnlySpan<char>.Empty;
		// If there is an anchor, then the query string must be inserted before its first occurrence.
		if (anchorIndex != -1)
		{
			anchorText = uriToBeAppended.Slice(anchorIndex);
			uriToBeAppended = uriToBeAppended.Slice(0, anchorIndex);
		}

		var queryIndex = uriToBeAppended.IndexOf('?');
		var hasQuery = queryIndex != -1;

		var sb = new StringBuilder();
		sb.Append(uriToBeAppended);
		foreach (var parameter in queryString)
		{
			if (parameter.Value == null)
			{
				continue;
			}

			sb.Append(hasQuery ? '&' : '?');
			sb.Append(UrlEncoder.Default.Encode(parameter.Key));
			sb.Append('=');
			sb.Append(UrlEncoder.Default.Encode(parameter.Value));
			hasQuery = true;
		}

		sb.Append(anchorText);
		return sb.ToString();
	}

	public void Dispose()
	{
		_httpClient.Dispose();
	}
}
