using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Mindbox.YandexTracker.Helpers;
using Mindbox.YandexTracker.JsonConverters;

namespace Mindbox.YandexTracker;

public sealed class YandexTrackerClient : IYandexTrackerClient
{
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _jsonOptions;

	public YandexTrackerClient(
		IOptionsMonitor<YandexTrackerClientOptions> options,
		IHttpClientFactory httpClientFactory)
	{
		ArgumentNullException.ThrowIfNull(options);
		ArgumentNullException.ThrowIfNull(httpClientFactory);

		_httpClient = CreateHttpClient(httpClientFactory, options.CurrentValue);

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

	private static HttpClient CreateHttpClient(
		IHttpClientFactory httpClientFactory,
		YandexTrackerClientOptions options)
	{
		var httpClient = httpClientFactory.CreateClient();

		httpClient.DefaultRequestHeaders.UserAgent.Add(
			new ProductInfoHeaderValue(
				"Mindbox.YandexTrackerClient",
				Assembly.GetExecutingAssembly().GetName().Version!.ToString()));
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", options.OAuthToken);
		httpClient.DefaultRequestHeaders.Add("X-Cloud-Org-ID", options.Organization);

		return httpClient;
	}

	public async Task<GetQueuesResponse> GetQueueAsync(
		string queueKey,
		QueueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(queueKey);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not QueueExpandData.None)
		{
			parameters["expand"] = expand.Value.ToYandexQueryString(QueueExpandData.All, QueueExpandData.None);
		}

		return await ExecuteYandexTrackerApiRequestAsync<GetQueuesResponse>(
			$"queues/{queueKey}",
			HttpMethod.Get,
			payload: null,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<GetQueuesResponse>> GetQueuesAsync(
		QueuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		var parameters = new Dictionary<string, string>();

		if (expand is not null && (QueuesExpandData)expand != QueuesExpandData.None)
		{
			parameters["expand"] = expand.Value.ToYandexQueryString(null, QueuesExpandData.None);
		}

		return await ExecuteYandexTrackerCollectionRequestAsync<GetQueuesResponse>(
			"queues",
			HttpMethod.Get,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	public async Task<GetIssueResponse> GetIssueAsync(
		string issueKey,
		IssueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(issueKey);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not IssueExpandData.None)
		{
			parameters["expand"] = expand.Value.ToYandexQueryString(null, IssueExpandData.None);
		}

		return await ExecuteYandexTrackerApiRequestAsync<GetIssueResponse>(
			$"issues/{issueKey}",
			HttpMethod.Get,
			payload: null!,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<GetIssueResponse>> GetIssuesFromQueueAsync(
		string queueKey,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(queueKey);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not IssuesExpandData.None)
		{
			parameters["expand"] = expand.Value.ToYandexQueryString(null, IssuesExpandData.None);
		}

		var request = new GetIssuesFromQueueRequest
		{
			Queue = queueKey
		};

		return await ExecuteYandexTrackerCollectionRequestAsync<GetIssueResponse>(
			"issues/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<GetIssueResponse>> GetIssuesByKeysAsync(
		IReadOnlyList<string> issueKeys,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(issueKeys);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not IssuesExpandData.None)
		{
			parameters["expand"] = expand.Value.ToYandexQueryString(null, IssuesExpandData.None);
		}

		var request = new GetIssuesFromKeysRequest
		{
			Keys = issueKeys
		};

		return await ExecuteYandexTrackerCollectionRequestAsync<GetIssueResponse>(
			"issues/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<GetIssueResponse>> GetIssuesByFilterAsync(
		GetIssuesByFilterRequest request,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not IssuesExpandData.None)
		{
			parameters["expand"] = expand.Value.ToYandexQueryString(null, IssuesExpandData.None);
		}

		return await ExecuteYandexTrackerCollectionRequestAsync<GetIssueResponse>(
			"issues/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<GetIssueResponse>> GetIssuesByQueryAsync(
		string query,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(query);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not IssuesExpandData.None)
		{
			parameters["expand"] = expand.Value.ToYandexQueryString(null, IssuesExpandData.None);
		}

		var request = new GetIssuesByQueryRequest
		{
			Query = query
		};

		return await ExecuteYandexTrackerCollectionRequestAsync<GetIssueResponse>(
			"issues/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	public async Task<CreateIssueResponse> CreateIssueAsync(
		CreateIssueRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		return await ExecuteYandexTrackerApiRequestAsync<CreateIssueResponse>(
			"issues",
			HttpMethod.Post,
			payload: request,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<GetComponentResponse>> GetComponentsAsync(
		CancellationToken cancellationToken = default)
	{
		return await ExecuteYandexTrackerCollectionRequestAsync<GetComponentResponse>(
			"components",
			HttpMethod.Get,
			payload: null!,
			cancellationToken: cancellationToken);
	}

	public async Task<CreateComponentResponse> CreateComponentAsync(
		CreateComponentRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		return await ExecuteYandexTrackerApiRequestAsync<CreateComponentResponse>(
			"components",
			HttpMethod.Post,
			payload: request,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<GetCommentsResponse>> GetCommentsAsync(
		string issueKey,
		CommentExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(issueKey);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not CommentExpandData.None)
		{
			parameters["expand"] = expand.Value.ToYandexQueryString(CommentExpandData.All, CommentExpandData.None);
		}

		return await ExecuteYandexTrackerCollectionRequestAsync<GetCommentsResponse>(
			$"issues/{issueKey}/comments",
			HttpMethod.Get,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	public async Task<CreateCommentResponse> CreateCommentAsync(
		string issueKey,
		CreateCommentRequest request,
		bool? addAuthorToFollowers = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(issueKey);
		ArgumentNullException.ThrowIfNull(request);

		var parameters = new Dictionary<string, string>();

		if (addAuthorToFollowers is not null)
		{
			parameters["isAddToFollowers"] = addAuthorToFollowers.ToString()!;
		}

		return await ExecuteYandexTrackerApiRequestAsync<CreateCommentResponse>(
			$"issues/{issueKey}/comments",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<GetAttachmentResponse>> GetAttachmentsAsync(
		string issueKey,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(issueKey);

		return await ExecuteYandexTrackerCollectionRequestAsync<GetAttachmentResponse>(
			$"issues/{issueKey}/attachments",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public async Task<CreateAttachmentResponse> CreateAttachmentAsync(
		string issueKey,
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(issueKey);
		ArgumentNullException.ThrowIfNull(fileStream);

		var parameters = new Dictionary<string, string>();

		if (newFileName is not null)
			parameters["filename"] = newFileName;

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

	public async Task<IReadOnlyList<string>> GetTagsAsync(
		string queueKey,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(queueKey);

		return await ExecuteYandexTrackerCollectionRequestAsync<string>(
			$"queues/{queueKey}/tags",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public async Task<CreateProjectResponse> CreateProjectAsync(
		ProjectEntityType entityType,
		CreateProjectRequest request,
		ProjectFieldData? returnedFields = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		var parameters = new Dictionary<string, string>();

		if (returnedFields is not null
			and not ProjectFieldData.None)
		{
			parameters["fields"] = returnedFields.Value.ToYandexQueryString(null, ProjectFieldData.None);
		}

		return await ExecuteYandexTrackerApiRequestAsync<CreateProjectResponse>(
			$"entities/{entityType.ToCamelCase()}",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<ProjectInfo>> GetProjectsAsync(
		ProjectEntityType entityType,
		GetProjectsRequest request,
		ProjectFieldData? returnedFields = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		var parameters = new Dictionary<string, string>();

		if (returnedFields is not null and not ProjectFieldData.None)
		{
			parameters["fields"] = returnedFields.Value.ToYandexQueryString(null, ProjectFieldData.None);
		}

		var page = 1;
		parameters["page"] = page.ToString(CultureInfo.InvariantCulture);
		parameters["perPage"] = "100";

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

		} while (response.Pages > page);

		return projects;
	}

	public async Task<IReadOnlyList<GetIssueFieldsResponse>> GetLocalQueueFieldsAsync(
		string queueKey,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(queueKey);

		try
		{
			return await ExecuteYandexTrackerCollectionRequestAsync<GetIssueFieldsResponse>(
				$"queues/{queueKey}/localFields",
				HttpMethod.Get,
				cancellationToken: cancellationToken);
		}
		catch (InvalidOperationException)
		{
			return [];
		}
	}

	public async Task<IReadOnlyList<GetIssueFieldsResponse>> GetGlobalFieldsAsync(CancellationToken cancellationToken = default)
	{
		return await ExecuteYandexTrackerCollectionRequestAsync<GetIssueFieldsResponse>(
			"fields",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public async Task<UserDetailedInfoDto> GetMyselfAsync(CancellationToken cancellationToken)
	{
		return await ExecuteYandexTrackerApiRequestAsync<UserDetailedInfoDto>(
			"myself",
			HttpMethod.Get,
			payload: null,
			cancellationToken: cancellationToken);
	}

	public async Task<UserDetailedInfoDto> GetUserByIdAsync(
		string userId,
		CancellationToken cancellationToken = default)
	{
		return await ExecuteYandexTrackerApiRequestAsync<UserDetailedInfoDto>(
			$"users/{userId}",
			HttpMethod.Get,
			payload: null,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<UserDetailedInfoDto>> GetUsersAsync(CancellationToken cancellationToken = default)
	{
		return await ExecuteYandexTrackerCollectionRequestAsync<UserDetailedInfoDto>(
			"users",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public Task<IReadOnlyList<GetIssueTypeResponse>> GetIssueTypesAsync(CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<GetIssueTypeResponse>(
			"issuetypes",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public Task<IReadOnlyList<GetResolutionResponse>> GetResolutionsAsync(CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<GetResolutionResponse>(
			"resolutions",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public Task<IReadOnlyList<GetIssueStatusResponse>> GetIssueStatusesAsync(CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<GetIssueStatusResponse>(
			"statuses",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public async Task<CreateQueueLocalFieldResponse> CreateLocalFieldInQueueAsync(
		string queueKey,
		CreateQueueLocalFieldRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(queueKey);
		ArgumentNullException.ThrowIfNull(request);

		return await ExecuteYandexTrackerApiRequestAsync<CreateQueueLocalFieldResponse>(
			$"queues/{queueKey}/localFields",
			HttpMethod.Post,
			payload: request,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<GetFieldCategoriesResponse>> GetFieldCategoriesAsync(
		CancellationToken cancellationToken = default)
	{
		return await ExecuteYandexTrackerCollectionRequestAsync<GetFieldCategoriesResponse>(
			"fields/categories",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public async Task<CreateQueueResponse> CreateQueueAsync(
		CreateQueueRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		return await ExecuteYandexTrackerApiRequestAsync<CreateQueueResponse>(
			"queues",
			HttpMethod.Post,
			payload: request,
			cancellationToken: cancellationToken);
	}

	public async Task DeleteQueueAsync(string queueKey, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(queueKey);

		await ExecuteYandexTrackerApiRequestAsync(
			$"queues/{queueKey}",
			HttpMethod.Delete,
			payload: null,
			cancellationToken: cancellationToken);
	}

	public async Task DeleteCommentAsync(
		string issueKey,
		int commentId,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(issueKey);

		await ExecuteYandexTrackerApiRequestAsync(
			$"issues/{issueKey}/comments/{commentId}",
			HttpMethod.Delete,
			payload: null,
			cancellationToken: cancellationToken);
	}

	public async Task DeleteAttachmentAsync(string issueKey, string attachmentKey, CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(issueKey);
		ArgumentException.ThrowIfNullOrWhiteSpace(attachmentKey);

		await ExecuteYandexTrackerApiRequestAsync(
			$"issues/{issueKey}/attachments/{attachmentKey}",
			HttpMethod.Delete,
			payload: null,
			cancellationToken: cancellationToken);
	}

	public async Task DeleteProjectAsync(
		ProjectEntityType entityType,
		int projectShortId,
		bool? deleteWithBoard = null,
		CancellationToken cancellationToken = default)
	{
		var parameters = new Dictionary<string, string>();

		if (deleteWithBoard is not null)
			parameters["withBoard"] = deleteWithBoard.ToString()!;

		await ExecuteYandexTrackerApiRequestAsync(
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
			requestUri = new Uri(QueryHelpers.AddQueryString(requestUri.ToString(), parameters!));
		}

		return await RetryHelpers.RetryOnExceptionAsync(
			ExecuteAndProcessResultAsync,
			retryCount: 4,
			cancellationToken);

		async Task<HttpResponseMessage> ExecuteAndProcessResultAsync()
		{
			var request = new HttpRequestMessage(method, requestUri);

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

			await CheckRateLimitExceededAsync(response, cancellationToken);

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

		static async Task CheckRateLimitExceededAsync(
			HttpResponseMessage response,
			CancellationToken cancellationToken)
		{
			var remaining = TryGetHeaderValue(response, "x-ratelimit-remaining");

			if (remaining is not "0")
				return;

			var retrySeconds = TryGetHeaderValue(response, "Retry-After")?.Transform(int.Parse) ?? 1;
			var retryPeriod = TimeSpan.FromSeconds(retrySeconds + 1);
			if (retryPeriod.TotalSeconds > TimeSpan.FromMinutes(1).TotalSeconds)
			{
				var noRetryException = new YandexTrackerException(
					"YandexTracker rate limit reached. Too long wait for next try.",
					response.StatusCode);
				noRetryException.Data.Add("Retry-After", retrySeconds);

				throw noRetryException;
			}

			await Task.Delay(retryPeriod, cancellationToken);

			var exception =
				new InvalidOperationException($"Request was not successful: {response.StatusCode} : {response.Content}");

			exception.Data.Add("x-ratelimit-remaining", remaining);
			exception.Data.Add("x-ratelimit-limit", TryGetHeaderValue(response, "x-ratelimit-limit") ?? "null");
			exception.Data.Add("x-ratelimit-used", TryGetHeaderValue(response, "x-ratelimit-used") ?? "null");
			exception.Data.Add("x-ratelimit-reset", TryGetHeaderValue(response, "x-ratelimit-resett") ?? "null");

			throw exception;
		}
	}

	private static string? TryGetHeaderValue(HttpResponseMessage response, string header)
		=> response.Headers
			.Where(h => h.Key.Equals(header, StringComparison.OrdinalIgnoreCase))
			.Select(h => h.Value)
			.SingleOrDefault()
			?.SingleOrDefault()
			?.TrimAndMakeNullIfEmpty();

	private async Task<IReadOnlyList<TResult>> ExecuteYandexTrackerCollectionRequestAsync<TResult>(
		string requestTo,
		HttpMethod httpMethod,
		object? payload = null,
		IDictionary<string, string>? parameters = null,
		IDictionary<string, string>? headers = null,
		CancellationToken cancellationToken = default)
	{
		const int perPage = 100;

		var pageNumber = 1;

		var result = new List<TResult>();
		List<TResult> dataChunk;
		do
		{
			var parametersWithPaging = parameters is not null
				? new Dictionary<string, string>(parameters)
				: [];

			parametersWithPaging["perPage"] = perPage.ToString(CultureInfo.InvariantCulture);
			parametersWithPaging["page"] = pageNumber.ToString(CultureInfo.InvariantCulture);

			dataChunk = await ExecuteYandexTrackerApiRequestAsync<List<TResult>>(
				requestTo,
				httpMethod,
				payload: payload,
				parametersWithPaging,
				headers,
				cancellationToken);

			result.AddRange(dataChunk);

			pageNumber++;
		}
		while (dataChunk.Count == perPage);

		return result;
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

	public void Dispose()
	{
		_httpClient.Dispose();
	}
}
