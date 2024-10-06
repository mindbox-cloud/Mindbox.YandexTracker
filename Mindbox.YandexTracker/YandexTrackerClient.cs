using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Mindbox.YandexTracker;

public sealed class YandexTrackerClient : IYandexTrackerClient
{
	private readonly IOptionsMonitor<YandexTrackerClientOptions> _options;
	private readonly HttpClient _httpClient;

	public YandexTrackerClient(
		IOptionsMonitor<YandexTrackerClientOptions> options,
		IHttpClientFactory httpClientFactory)
	{
		_options = options;
		_httpClient = httpClientFactory.CreateClient();

		_httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("YandexTrackerClient", "1"));
		_httpClient.DefaultRequestHeaders.Host = "api.tracker.yandex.net";
	}

	public async Task<Queue> GetQueueAsync(
		string queueKey,
		QueueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(queueKey);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not QueueExpandData.None)
		{
			parameters["expand"] = expand is QueueExpandData.All
				? expand.ToString()!
				: ((QueueExpandData)expand).ToQueryString();
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
			parameters["expand"] = ((QueuesExpandData)expand).ToQueryString();
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
		ArgumentNullException.ThrowIfNull(issueKey);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not IssueExpandData.None)
		{
			parameters["expand"] = ((IssueExpandData)expand).ToQueryString();
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

	public async Task<IReadOnlyList<Issue>> GetIssuesAsync(
		GetIssuesRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		var parameters = new Dictionary<string, string>();

		if (request.Expand is not null and not IssuesExpandData.None)
		{
			parameters["expand"] = ((IssuesExpandData)request.Expand).ToQueryString();
		}

		var issueTypeInfos = (await GetIssueTypesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

		var issueStatusInfos = (await GetIssueStatusesAsync(cancellationToken))
			.ToDictionary(dto => dto.Key, dto => dto);

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
		CreateIssueRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

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

	public async Task<IReadOnlyList<Comment>> GetCommentsAsync(
		string issueKey,
		CommentExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(issueKey);

		var parameters = new Dictionary<string, string>();

		if (expand is not null and not CommentExpandData.None)
		{
			if (expand is CommentExpandData.All)
				parameters["expand"] = expand.ToString()!;
			else
				parameters["expand"] = ((CommentExpandData)expand).ToQueryString();
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
		CreateCommentRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(issueKey);
		ArgumentNullException.ThrowIfNull(request);

		var parameters = new Dictionary<string, string>();

		if (request.IsAddToFollowers is not null)
		{
			parameters["isAddToFollowers"] = request.IsAddToFollowers.ToString()!;
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
		ArgumentNullException.ThrowIfNull(issueKey);

		return (await ExecuteYandexTrackerCollectionRequestAsync<GetAttachmentResponse>(
			$"issues/{issueKey}/attachments",
			HttpMethod.Get,
			cancellationToken: cancellationToken))
			.Select(dto => dto.ToAttachment())
			.ToList();
	}

	public async Task<Attachment> CreateAttachmentAsync(
		string issueKey,
		byte[] file,
		string? newFileName = null,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(issueKey);
		ArgumentNullException.ThrowIfNull(file);

		var parameters = new Dictionary<string, string>();

		if (newFileName is not null)
			parameters["filename"] = newFileName;

		return (await ExecuteYandexTrackerApiRequestAsync<CreateAttachmentResponse>(
			$"issues/{issueKey}/attachments",
			HttpMethod.Post,
			payload: file,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.ToAttachment();
	}

	public async Task<IReadOnlyList<string>> GetTagsAsync(
		string queueKey,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(queueKey);

		return await ExecuteYandexTrackerCollectionRequestAsync<string>(
			$"queues/{queueKey}/tags",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public async Task<Project> CreateProjectAsync(
		ProjectEntityType entityType,
		CreateProjectRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		var parameters = new Dictionary<string, string>();

		if (request.FieldsWhichIncludedInResponse is not null
			and not ProjectFieldData.None)
		{
			parameters["fields"] = ((ProjectFieldData)request.FieldsWhichIncludedInResponse).ToQueryString();
		}

		return (await ExecuteYandexTrackerApiRequestAsync<CreateProjectResponse>(
			$"entities/{entityType}",
			HttpMethod.Post,
			payload: request,
			parameters: parameters,
			cancellationToken: cancellationToken))
			.ToProject();
	}

	public async Task<IReadOnlyList<Project>> GetProjectsAsync(
		ProjectEntityType entityType,
		GetProjectsRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		var parameters = new Dictionary<string, string>();

		if (request.FieldsWhichIncludedInResponse is not null
			and not ProjectFieldData.None)
		{
			parameters["fields"] = ((ProjectFieldData)request.FieldsWhichIncludedInResponse).ToQueryString();
		}

		return (await ExecuteYandexTrackerApiRequestAsync<GetProjectsResponse>(
			$"entities/{entityType}/_search",
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
		ArgumentNullException.ThrowIfNull(queueKey);

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
		}

		return [.. globalFields, .. localQueueFields];
	}

	public async Task<IReadOnlyList<GetUserResponse>> GetUsersAsync(CancellationToken cancellationToken = default)
	{
		return await ExecuteYandexTrackerCollectionRequestAsync<GetUserResponse>(
			"users",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<GetIssueTypeResponse>> GetIssueTypesAsync(
		CancellationToken cancellationToken = default)
	{
		return await ExecuteYandexTrackerCollectionRequestAsync<GetIssueTypeResponse>(
			"issuetypes",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<GetResolutionResponse>> GetResolutionsAsync(
		CancellationToken cancellationToken = default)
	{
		return await ExecuteYandexTrackerCollectionRequestAsync<GetResolutionResponse>(
			"resolutions",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyList<GetIssueStatusResponse>> GetIssueStatusesAsync(
		CancellationToken cancellationToken = default)
	{
		return await ExecuteYandexTrackerCollectionRequestAsync<GetIssueStatusResponse>(
			"statuses",
			HttpMethod.Get,
			cancellationToken: cancellationToken);
	}

	public async Task<Queue> CreateQueueAsync(
		CreateQueueRequest request,
		CancellationToken cancellationToken = default)
	{
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
		ArgumentNullException.ThrowIfNull(queueKey);

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
		ArgumentNullException.ThrowIfNull(issueKey);

		await ExecuteYandexTrackerApiRequestAsync(
			$"issues/{issueKey}/comments/{commentId}",
			HttpMethod.Delete,
			payload: null,
			cancellationToken: cancellationToken);
	}

	public async Task DeleteAttachmentAsync(string issueKey, string attachmentKey, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(issueKey);
		ArgumentNullException.ThrowIfNull(attachmentKey);

		await ExecuteYandexTrackerApiRequestAsync(
			$"issues/{issueKey}/attachments/{attachmentKey}",
			HttpMethod.Delete,
			payload: null,
			cancellationToken: cancellationToken);
	}

	public async Task DeleteProjectAsync(
		ProjectEntityType entityType,
		int projectShortId,
		CancellationToken cancellationToken = default)
	{
		await ExecuteYandexTrackerApiRequestAsync(
			$"entities/{entityType}/{projectShortId}",
			HttpMethod.Delete,
			payload: null,
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
		var optionsSnapshot = _options.CurrentValue;

		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", optionsSnapshot.Token);

		if (_httpClient.DefaultRequestHeaders.Contains("X-Cloud-Org-ID"))
		{
			_httpClient.DefaultRequestHeaders.Remove("X-Cloud-Org-ID");
		}

		_httpClient.DefaultRequestHeaders.Add("X-Cloud-Org-ID", optionsSnapshot.Organization);

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

		async Task<TResult> ExecuteAndProcessResultAsync()
		{
			var request = new HttpRequestMessage(method, requestUri);

			if (payload is not null)
			{
				request.Content = new StringContent(
					JsonConvert.SerializeObject(payload),
					Encoding.UTF8,
					"application/json");
			}

			if (headers is not null)
			{
				foreach (var header in headers)
				{
					request.Headers.Add(header.Key, header.Value);
				}
			}

			var response = await _httpClient.SendAsync(request, cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				await CheckRateLimitExceededAsync(response, cancellationToken);

				throw new InvalidOperationException($"Request was not successful: {response.StatusCode} : {response.Content}");
			}

			var resultContent = await response.Content.ReadAsStringAsync(cancellationToken);
			return JsonConvert.DeserializeObject<TResult>(resultContent)!;
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
		bool withPagination = false,
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

			if (withPagination)
			{
				parametersWithPaging["perPage"] = perPage.ToString(CultureInfo.InvariantCulture);
				parametersWithPaging["page"] = pageNumber.ToString(CultureInfo.InvariantCulture);
			}

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
		while (dataChunk.Count < perPage && withPagination);

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
		var optionsSnapshot = _options.CurrentValue;

		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", optionsSnapshot.Token);

		if (_httpClient.DefaultRequestHeaders.Contains("X-Cloud-Org-ID"))
		{
			_httpClient.DefaultRequestHeaders.Remove("X-Cloud-Org-ID");
		}

		_httpClient.DefaultRequestHeaders.Add("X-Cloud-Org-ID", optionsSnapshot.Organization);

		var apiVersion = "v2";

		var effectivePath = $"{apiVersion}/{requestTo}";
		var baseAddress = new Uri("https://api.tracker.yandex.net/");
		var requestUri = new Uri(baseAddress, effectivePath);
		if (parameters is not null)
		{
			requestUri = new Uri(QueryHelpers.AddQueryString(requestUri.ToString(), parameters!));
		}

		await RetryHelpers.RetryOnExceptionAsync(
			ExecuteAndProcessResultAsync,
			retryCount: 4,
			cancellationToken);

		async Task ExecuteAndProcessResultAsync()
		{
			var request = new HttpRequestMessage(method, requestUri);

			if (payload is not null)
			{
				request.Content = new StringContent(
					JsonConvert.SerializeObject(payload),
					Encoding.UTF8,
					"application/json");
			}

			if (headers is not null)
			{
				foreach (var header in headers)
				{
					request.Headers.Add(header.Key, header.Value);
				}
			}

			var response = await _httpClient.SendAsync(request, cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				await CheckRateLimitExceededAsync(response, cancellationToken);

				throw new InvalidOperationException($"Request was not successful: {response.StatusCode} : {response.Content}");
			}
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
}
