using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Linq;
using System.Globalization;

namespace Mindbox.YandexTracker;
public sealed class GitlabClient(
	IOptionsMonitor<GitlabClientOptions> options,
	IHttpClientFactory httpClientFactory) : IGitlabClient
{
	public async Task<IReadOnlyCollection<Issue>> GetIssuesAsync(string repositoryName)
	{
		ArgumentNullException.ThrowIfNull(repositoryName);

		var parameters = new Dictionary<string, string>
		{
			["expand"] = null! // todo
		};

		return (await ExecuteGitlabApiRequestAsync<List<IssueDto>>(
			"issues",
			"_search",
			HttpMethod.Post,
			payload: null!,
			parameters
			))
			.Select(dto => dto.ToIssue())
			.ToArray();
	}

	private async Task<TResult> ExecuteGitlabApiRequestAsync<TResult>(
		string requestTo,
		string relativePath,
		HttpMethod method,
		object payload,
		IDictionary<string, string>? parameters = null,
		IDictionary<string, string>? headers = null)
		where TResult : class, new()
	{
		var optionsSnapshot = options.CurrentValue;

		using var httpClient = httpClientFactory.CreateClient();
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", optionsSnapshot.Token);
		httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GitlabClient", "1"));

		var apiVersion = "v2";

		var effectivePath = $"{apiVersion}/{requestTo}/{relativePath}";
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
				var noRetryException = new NoRetryException("Gitlab rate limit reached. Too long wait for next try.");
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
			.TrimAndMakeNullIfEmpty();

	private async Task<IEnumerable<TResult>> ExecuteGitlabCollectionRequestAsync<TResult>(
		string requestTo,
		string relativePath,
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
				relativePath,
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
