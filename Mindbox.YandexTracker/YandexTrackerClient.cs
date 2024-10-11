using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Mindbox.YandexTracker.Helpers;
using Newtonsoft.Json;

namespace Mindbox.YandexTracker;

public sealed class YandexTrackerClient : IYandexTrackerClient
{
	private readonly HttpClient _httpClient;

	public YandexTrackerClient(
		IOptionsMonitor<YandexTrackerClientOptions> options,
		IHttpClientFactory httpClientFactory)
	{
		ArgumentNullException.ThrowIfNull(options);
		ArgumentNullException.ThrowIfNull(httpClientFactory);

		_httpClient = CreateHttpClient(httpClientFactory, options.CurrentValue);
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

	public async Task<Queue> GetQueueAsync(
		string queueKey,
		QueueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(queueKey);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not QueueExpandData.None)
		{
			parameters["expand"] = expand is QueueExpandData.All
				? expand.ToString()!
				: expand.Value.ToQueryString();
		}

		var issueTypeInfos = (await GetIssueTypesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		var resolutionInfos = (await GetResolutionsAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		return (await ExecuteYandexTrackerApiRequestAsync<GetQueuesResponse>(
			$"queues/{queueKey}",
			HttpMethod.Get,
			payload: null,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.ToQueue(issueTypeInfos, resolutionInfos);
	}

	public async Task<IReadOnlyList<Queue>> GetQueuesAsync(
		QueuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		var parameters = new Dictionary<string, string>();

		if (expand is not null && (QueuesExpandData)expand != QueuesExpandData.None)
		{
			parameters["expand"] = expand.Value.ToQueryString();
		}

		var issueTypeInfos = (await GetIssueTypesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		var resolutionInfos = (await GetResolutionsAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		return (await ExecuteYandexTrackerCollectionRequestAsync<GetQueuesResponse>(
			"queues",
			HttpMethod.Get,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.Select(dto => dto.ToQueue(issueTypeInfos, resolutionInfos))
			.ToList();
	}

	public async Task<Issue> GetIssueAsync(
		string issueKey,
		IssueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(issueKey);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not IssueExpandData.None)
		{
			parameters["expand"] = expand.Value.ToQueryString();
		}

		var issueTypeInfos = (await GetIssueTypesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		var issueStatusInfos = (await GetIssueStatusesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		return (await ExecuteYandexTrackerApiRequestAsync<GetIssueResponse>(
			$"issues/{issueKey}",
			HttpMethod.Get,
			payload: null!,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.ToIssue(issueTypeInfos, issueStatusInfos);
	}

	public async Task<IReadOnlyList<Issue>> GetIssuesFromQueueAsync(
		string queueKey,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(queueKey);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not IssuesExpandData.None)
		{
			parameters["expand"] = expand.Value.ToQueryString();
		}

		var issueTypeInfos = (await GetIssueTypesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		var issueStatusInfos = (await GetIssueStatusesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		var request = new GetIssuesFromQueueRequest
		{
			QueueKey = queueKey
		};

		return (await ExecuteYandexTrackerCollectionRequestAsync<GetIssueResponse>(
			"issues/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.Select(dto => dto.ToIssue(issueTypeInfos, issueStatusInfos))
			.ToList();
	}

	public async Task<IReadOnlyList<Issue>> GetIssuesByKeysAsync(
		IReadOnlyList<string> keys,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(keys);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not IssuesExpandData.None)
		{
			parameters["expand"] = expand.Value.ToQueryString();
		}

		var issueTypeInfos = (await GetIssueTypesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		var issueStatusInfos = (await GetIssueStatusesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		var request = new GetIssuesFromKeysRequest
		{
			Keys = new Collection<string>([.. keys])
		};

		return (await ExecuteYandexTrackerCollectionRequestAsync<GetIssueResponse>(
			"issues/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.Select(dto => dto.ToIssue(issueTypeInfos, issueStatusInfos))
			.ToList();
	}

	public async Task<IReadOnlyList<Issue>> GetIssuesByFilterAsync(
		IssuesFilter issuesFilter,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(issuesFilter);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not IssuesExpandData.None)
		{
			parameters["expand"] = expand.Value.ToQueryString();
		}

		var issueTypeInfos = (await GetIssueTypesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		var issueStatusInfos = (await GetIssueStatusesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		var request = new GetIssuesByFilterRequest
		{
			Filter = issuesFilter.ToDictionary()
		};

		return (await ExecuteYandexTrackerCollectionRequestAsync<GetIssueResponse>(
			"issues/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.Select(dto => dto.ToIssue(issueTypeInfos, issueStatusInfos))
			.ToList();
	}

	public async Task<IReadOnlyList<Issue>> GetIssuesByQueryAsync(
		string query,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(query);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not IssuesExpandData.None)
		{
			parameters["expand"] = expand.Value.ToQueryString();
		}

		var issueTypeInfos = (await GetIssueTypesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		var issueStatusInfos = (await GetIssueStatusesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		var request = new GetIssuesByQueryRequest
		{
			Query = query
		};

		return (await ExecuteYandexTrackerCollectionRequestAsync<GetIssueResponse>(
			"issues/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.Select(dto => dto.ToIssue(issueTypeInfos, issueStatusInfos))
			.ToList();
	}

	public async Task<Issue> CreateIssueAsync(
		Issue issue,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(issue);

		var request = issue.ToCreateIssueRequest();

		var issueTypeInfos = (await GetIssueTypesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		var issueStatusInfos = (await GetIssueStatusesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		return (await ExecuteYandexTrackerApiRequestAsync<CreateIssueResponse>(
			"issues",
			HttpMethod.Post,
			payload: request,
			cancellationToken: cancellationToken))
			.ToIssue(issueTypeInfos, issueStatusInfos);
	}

	public async Task<IReadOnlyList<Component>> GetComponentsAsync(
		CancellationToken cancellationToken = default)
	{
		return (await ExecuteYandexTrackerApiRequestAsync<List<GetComponentResponse>>(
			"components",
			HttpMethod.Get,
			payload: null!,
			cancellationToken: cancellationToken))
			.Select(dto => dto.ToComponent())
			.ToList();
	}

	public async Task<Component> CreateComponentAsync(
		string componentName,
		string queueKey,
		string? description = null,
		string? leadLogin = null,
		bool? assignAuto = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(componentName);
		ArgumentException.ThrowIfNullOrWhiteSpace(queueKey);

		var request = new CreateComponentRequest
		{
			Name = componentName,
			Queue = queueKey
		};

		var parameters = new Dictionary<string, string>();

		if (description is not null)
			parameters["description"] = description;

		if (leadLogin is not null)
			parameters["lead"] = leadLogin;

		if (assignAuto is not null)
			parameters["assignAuto"] = assignAuto.ToString()!;

		return (await ExecuteYandexTrackerApiRequestAsync<CreateComponentResponse>(
			"components",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.ToComponent();
	}

	public async Task<IReadOnlyList<Comment>> GetCommentsAsync(
		string issueKey,
		CommentExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(issueKey);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not CommentExpandData.None)
		{
			if (expand is CommentExpandData.All)
				parameters["expand"] = expand.ToString()!;
			else
				parameters["expand"] = expand.Value.ToQueryString();
		}

		return (await ExecuteYandexTrackerCollectionRequestAsync<GetCommentsResponse>(
			$"issues/{issueKey}/comments",
			HttpMethod.Get,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.Select(dto => dto.ToComment())
			.ToList();
	}

	public async Task<Comment> CreateCommentAsync(
		string issueKey,
		Comment comment,
		bool? addAuthorToFollowers = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(issueKey);
		ArgumentNullException.ThrowIfNull(comment);

		var request = comment.ToCreateCommentRequest();

		var parameters = new Dictionary<string, string>();

		if (addAuthorToFollowers is not null)
		{
			parameters["isAddToFollowers"] = addAuthorToFollowers.ToString()!;
		}

		return (await ExecuteYandexTrackerApiRequestAsync<CreateCommentResponse>(
			$"issues/{issueKey}/comments",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.ToComment();
	}

	public async Task<IReadOnlyList<Attachment>> GetAttachmentsAsync(
		string issueKey,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(issueKey);

		return (await ExecuteYandexTrackerCollectionRequestAsync<GetAttachmentResponse>(
			$"issues/{issueKey}/attachments",
			HttpMethod.Get,
			cancellationToken: cancellationToken))
			.Select(dto => dto.ToAttachment())
			.ToList();
	}

	public async Task<Attachment> CreateAttachmentAsync(
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

		return (await ExecuteYandexTrackerApiRequestAsync<CreateAttachmentResponse>(
			$"issues/{issueKey}/attachments",
			HttpMethod.Post,
			payload: form,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.ToAttachment();
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

	public async Task<Project> CreateProjectAsync(
		ProjectEntityType entityType,
		Project project,
		ProjectFieldData? returnedFields = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(project);

		var request = project.ToCreateProjectRequest();

		var parameters = new Dictionary<string, string>();

		if (returnedFields is not null
			and not ProjectFieldData.None)
		{
			parameters["fields"] = returnedFields.Value.ToQueryString();
		}

		return (await ExecuteYandexTrackerApiRequestAsync<CreateProjectResponse>(
			$"entities/{entityType.ToYandexRouteSegment()}",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.ToProject();
	}

	public async Task<IReadOnlyList<Project>> GetProjectsAsync(
		ProjectEntityType entityType,
		Project project,
		ProjectFieldData? returnedFields = null,
		string? input = null,
		string? orderBy = null,
		bool? orderAscending = null,
		bool? rootOnly = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(project);

		var parameters = new Dictionary<string, string>();

		if (returnedFields is not null and not ProjectFieldData.None)
		{
			parameters["fields"] = returnedFields.Value.ToQueryString();
		}

		var request = project.ToGetProjectsRequest(
			input,
			orderBy,
			orderAscending,
			rootOnly);

		return (await ExecuteYandexTrackerApiRequestAsync<GetProjectsResponse>(
			$"entities/{entityType.ToYandexRouteSegment()}/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.ToProjects();
	}

	public async Task<IReadOnlyList<IssueField>> GetAccessibleFieldsForIssueAsync(
		string queueKey,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(queueKey);

		var globalFields = (await ExecuteYandexTrackerCollectionRequestAsync<GetIssueFieldsResponse>(
			"fields",
			HttpMethod.Get,
			cancellationToken: cancellationToken))
			.Select(dto => dto.ToIssueField())
			.ToList();

		List<IssueField> localQueueFields = [];

		try
		{
			localQueueFields = (await ExecuteYandexTrackerCollectionRequestAsync<GetIssueFieldsResponse>(
				$"queues/{queueKey}/localFields",
				HttpMethod.Get,
				cancellationToken: cancellationToken))
				.Select(dto => dto.ToIssueField())
				.ToList();
		}
		catch (InvalidOperationException)
		{
			// Если локальных полей нет - InvalidOperationException
		}

		return [.. globalFields, .. localQueueFields];
	}

	public Task<UserDetailedInfo> GetMyselfAsync(CancellationToken cancellationToken)
	{
		return ExecuteYandexTrackerApiRequestAsync<UserDetailedInfo>(
			"myself",
			HttpMethod.Get,
			payload: null,
			cancellationToken: cancellationToken);
	}

	public Task<UserDetailedInfo> GetUserByIdAsync(
		string userId,
		CancellationToken cancellationToken = default)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(userId);

		return ExecuteYandexTrackerApiRequestAsync<UserDetailedInfo>(
			$"users/{userId}",
			HttpMethod.Get,
			payload: null,
			cancellationToken: cancellationToken);
	}

	public Task<IReadOnlyList<UserDetailedInfo>> GetUsersAsync(CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<UserDetailedInfo>(
			"users",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public Task<IReadOnlyList<IssueType>> GetIssueTypesAsync(CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<IssueType>(
			"issuetypes",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public Task<IReadOnlyList<Resolution>> GetResolutionsAsync(CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<Resolution>(
			"resolutions",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public Task<IReadOnlyList<IssueStatus>> GetIssueStatusesAsync(CancellationToken cancellationToken = default)
	{
		return ExecuteYandexTrackerCollectionRequestAsync<IssueStatus>(
			"statuses",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public async Task<Queue> CreateQueueAsync(
		Queue queue,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(queue);

		var request = queue.ToCreateQueueRequest();

		var issueTypeInfos = (await GetIssueTypesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		return (await ExecuteYandexTrackerApiRequestAsync<CreateQueueResponse>(
			"queues",
			HttpMethod.Post,
			payload: request,
			cancellationToken: cancellationToken))
			.ToQueue(issueTypeInfos);
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
			$"entities/{entityType.ToYandexRouteSegment()}/{projectShortId}",
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
		return JsonConvert.DeserializeObject<TResult>(resultContent)!;
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
					request.Content = new StringContent(
						JsonConvert.SerializeObject(payload),
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
			throw new InvalidOperationException($"Request was not successful: {response.StatusCode} : {errorMessage}");
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
				var noRetryException = new YandexTrackerRetryLimitExceededException(
					"YandexTracker rate limit reached. Too long wait for next try.");
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
