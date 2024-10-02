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
public sealed class YandexTrackerClient(
	IOptionsMonitor<YandexTrackerClientOptions> options,
	IHttpClientFactory httpClientFactory) : IYandexTrackerClient
{
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

		return (await ExecuteGitlabApiRequestAsync<GetQueuesResponse>(
			$"queues/{queueKey}",
			HttpMethod.Get,
			payload: null!,
			parameters: parameters))
			.ToQueue();
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

		return (await ExecuteGitlabCollectionRequestAsync<GetQueuesResponse>(
			"queues",
			parameters: parameters))
			.Select(dto => dto.ToQueue())
			.ToList();
	}

	public async Task<IReadOnlyList<Queue>> GetQueuesAsync(
		QueueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		var parameters = new Dictionary<string, string>();

		if (expand is not null and not QueueExpandData.None)
		{
			if (expand is QueueExpandData.All)
				parameters["expand"] = expand.ToString()!;
			else
				parameters["expand"] = ((QueueExpandData)expand).ToQueryString();
		}

		return (await ExecuteGitlabCollectionRequestAsync<GetQueuesResponse>(
			"queues",
			parameters: parameters))
			.Select(dto => dto.ToQueue())
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

		return (await ExecuteGitlabApiRequestAsync<GetIssueResponse>(
			$"issues/{issueKey}",
			HttpMethod.Get,
			payload: null!,
			parameters: parameters))
			.ToIssue();
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

		return (await ExecuteGitlabApiRequestAsync<List<GetIssueResponse>>(
			"issues/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters))
			.Select(dto => dto.ToIssue())
			.ToList();
	}

	public async Task<Issue> CreateIssueAsync(
		CreateIssueRequest request,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);

		return (await ExecuteGitlabApiRequestAsync<CreateIssueResponse>(
			"issues",
			HttpMethod.Post,
			payload: request))
			.ToIssue();
	}

	public async Task<IReadOnlyList<Component>> GetComponentsAsync(
		CancellationToken cancellationToken = default)
	{
		return (await ExecuteGitlabApiRequestAsync<List<GetComponentResponse>>(
			"components",
			HttpMethod.Get,
			payload: null!))
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

		return (await ExecuteGitlabCollectionRequestAsync<GetCommentsResponse>(
			$"issues/{issueKey}/comments",
			parameters: parameters))
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

		return (await ExecuteGitlabApiRequestAsync<CreateCommentResponse>(
			$"issues/{issueKey}/comments",
			HttpMethod.Post,
			payload: request,
			parameters: parameters))
			.ToComment();
	}

	public async Task<IReadOnlyList<Attachment>> GetAttachmentsAsync(
		string issueKey,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(issueKey);

		return (await ExecuteGitlabCollectionRequestAsync<GetAttachmentResponse>($"issues/{issueKey}/attachments"))
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

		return (await ExecuteGitlabApiRequestAsync<CreateAttachmentResponse>(
			$"issues/{issueKey}/attachments",
			HttpMethod.Post,
			payload: file,
			parameters: parameters))
			.ToAttachment();
	}

	public async Task<IReadOnlyList<string>> GetTagsAsync(
		string queueKey,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(queueKey);

		return (await ExecuteGitlabCollectionRequestAsync<string>($"queues/{queueKey}/tags"))
			.ToList();
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

		return (await ExecuteGitlabApiRequestAsync<CreateProjectResponse>(
			$"entities/{entityType}",
			HttpMethod.Post,
			payload: request,
			parameters: parameters))
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

		return (await ExecuteGitlabApiRequestAsync<GetProjectsResponse>(
			$"entities/{entityType}/_search",
			HttpMethod.Post,
			payload: request,
			parameters: parameters))
			.ToProjects();
	}

	public async Task<IReadOnlyList<IssueField>> GetAccessibleFieldsForIssueAsync(
		string queueKey,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(queueKey);

		var globalFields = (await ExecuteGitlabCollectionRequestAsync<GetIssueFieldsResponse>("fields"))
			.Select(dto => dto.ToIssueField())
			.ToList();

		var localQuqueFields = (await ExecuteGitlabCollectionRequestAsync<GetIssueFieldsResponse>(
			$"queues/{queueKey}/localFields"))
			.Select(dto => dto.ToIssueField())
			.ToList();

		return [.. globalFields, .. localQuqueFields];
	}

	public async Task<IReadOnlyList<GetUserResponse>> GetUsersAsync(CancellationToken cancellationToken = default)
	{
		return (await ExecuteGitlabCollectionRequestAsync<GetUserResponse>("users"))
			.ToList();
	}

	private async Task<TResult> ExecuteGitlabApiRequestAsync<TResult>(
		string requestTo,
		HttpMethod method,
		object payload,
		IDictionary<string, string>? parameters = null,
		IDictionary<string, string>? headers = null)
		where TResult : class
	{
		var optionsSnapshot = options.CurrentValue;

		using var httpClient = httpClientFactory.CreateClient();
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", optionsSnapshot.Token);
		httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("YandexTrackerClient", "1"));

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
			retryCount: 4);

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

			var response = await httpClient.SendAsync(request);

			if (!response.IsSuccessStatusCode)
			{
				CheckRateLimitExceeded(response);

				throw new InvalidOperationException($"Request was not successful: {response.StatusCode} : {response.Content}");
			}

			var resultContent = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<TResult>(resultContent)!;
		}

		static void CheckRateLimitExceeded(HttpResponseMessage response)
		{
			var remaining = TryGetHeaderValue(response, "x-ratelimit-remaining");

			if (remaining is not "0")
				return;

			var retrySeconds = TryGetHeaderValue(response, "Retry-After")?.Transform(int.Parse) ?? 1;
			var retryPeriod = TimeSpan.FromSeconds(retrySeconds + 1);
			if (retryPeriod.TotalSeconds > TimeSpan.FromMinutes(1).TotalSeconds)
			{
				var noRetryException = new NoRetryException("YandexTracker rate limit reached. Too long wait for next try.");
				noRetryException.Data.Add("Retry-After", retrySeconds);

				throw noRetryException;
			}

			Thread.Sleep(retryPeriod);

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

	private async Task<IEnumerable<TResult>> ExecuteGitlabCollectionRequestAsync<TResult>(
		string requestTo,
		IDictionary<string, string>? parameters = null,
		IDictionary<string, string>? headers = null)
	{
		const int perPage = 100;

		var pageNumber = 1;

		var result = new List<TResult>();
		while (true)
		{
			var parametersWithPaging = parameters is not null
				? new Dictionary<string, string>(parameters)
				: [];

			parametersWithPaging["perPage"] = perPage.ToString(CultureInfo.InvariantCulture);
			parametersWithPaging["page"] = pageNumber.ToString(CultureInfo.InvariantCulture);

			var resultList = await ExecuteGitlabApiRequestAsync<List<TResult>>(
				requestTo,
				HttpMethod.Get,
				payload: null!,
				parametersWithPaging,
				headers);

			result.AddRange(resultList);

			if (resultList.Count < perPage)
			{
				break;
			}

			pageNumber++;
		}

		return result;
	}
}
