using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Mindbox.YandexTracker.Template;

public sealed class YandexTrackerClientCachingDecorator(
	IYandexTrackerClient yandexTrackerClient,
	IMemoryCache cache,
	IOptionsMonitor<YandexTrackerClientCachingOptions> options) : IYandexTrackerClient
{
	public Task<CreateAttachmentResponse> CreateAttachmentAsync(
		string issueKey,
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateAttachmentAsync(issueKey, fileStream, newFileName, cancellationToken);
	}

	public Task<CreateAttachmentResponse> CreateTemporaryAttachmentAsync(
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateTemporaryAttachmentAsync(fileStream, newFileName, cancellationToken);
	}

	public Task<CreateCommentResponse> CreateCommentAsync(
		string issueKey,
		CreateCommentRequest request,
		bool? addAuthorToFollowers = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateCommentAsync(issueKey, request, addAuthorToFollowers, cancellationToken);
	}

	public Task<CreateIssueResponse> CreateIssueAsync(CreateIssueRequest request, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateIssueAsync(request, cancellationToken);
	}

	public Task<CreateProjectResponse> CreateProjectAsync(
		ProjectEntityType entityType,
		CreateProjectRequest request,
		ProjectFieldData? returnedFields = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateProjectAsync(entityType, request, returnedFields, cancellationToken);
	}

	public Task<CreateQueueResponse> CreateQueueAsync(CreateQueueRequest request, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateQueueAsync(request, cancellationToken);
	}

	public Task DeleteAttachmentAsync(string issueKey, string attachmentKey, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.DeleteAttachmentAsync(issueKey, attachmentKey, cancellationToken);
	}

	public Task DeleteCommentAsync(string issueKey, int commentId, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.DeleteCommentAsync(issueKey, commentId, cancellationToken);
	}

	public Task DeleteProjectAsync(
		ProjectEntityType entityType,
		int projectShortId,
		bool? deleteWithBoard = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.DeleteProjectAsync(entityType, projectShortId, deleteWithBoard, cancellationToken);
	}

	public Task DeleteQueueAsync(string queueKey, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.DeleteQueueAsync(queueKey, cancellationToken);
	}

	public Task<IReadOnlyList<GetIssueFieldsResponse>> GetLocalQueueFieldsAsync(
		string queueKey,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_localFields_{queueKey}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetLocalQueueFieldsAsync(queueKey, paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public Task<IReadOnlyList<GetIssueFieldsResponse>> GetGlobalFieldsAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_globalFields";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetGlobalFieldsAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public Task<UserDetailedInfoDto> GetMyselfAsync(CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetMyselfAsync(cancellationToken);
	}

	public Task<IReadOnlyList<GetAttachmentResponse>> GetAttachmentsAsync(
		string issueKey,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetAttachmentsAsync(issueKey, paginationSettings, cancellationToken);
	}

	public Task<CreateComponentResponse> CreateComponentAsync(
		CreateComponentRequest request,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateComponentAsync(
			request,
			cancellationToken);
	}

	public Task<IReadOnlyList<GetCommentsResponse>> GetCommentsAsync(
		string issueKey,
		CommentExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetCommentsAsync(issueKey, expand, paginationSettings, cancellationToken);
	}

	public Task<IReadOnlyList<GetComponentResponse>> GetComponentsAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_components";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetComponentsAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public Task<GetIssueResponse> GetIssueAsync(
		string issueKey,
		IssueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetIssueAsync(issueKey, expand, cancellationToken);
	}

	public Task<IReadOnlyList<GetIssueResponse>> GetIssuesByFilterAsync(
		GetIssuesByFilterRequest request,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetIssuesByFilterAsync(request, expand, paginationSettings, cancellationToken);
	}

	public Task<IReadOnlyList<GetIssueResponse>> GetIssuesByQueryAsync(
		string query,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetIssuesByQueryAsync(query, expand, paginationSettings, cancellationToken);
	}

	public Task<IReadOnlyList<GetIssueResponse>> GetIssuesByKeysAsync(
		IReadOnlyList<string> issueKeys,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetIssuesByKeysAsync(issueKeys, expand, paginationSettings, cancellationToken);
	}

	public Task<IReadOnlyList<GetIssueResponse>> GetIssuesFromQueueAsync(
		string queueKey,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetIssuesFromQueueAsync(queueKey, expand, paginationSettings, cancellationToken);
	}

	public Task<IReadOnlyList<GetIssueStatusResponse>> GetIssueStatusesAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_issueStatuses";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetIssueStatusesAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public Task<CreateQueueLocalFieldResponse> CreateLocalFieldInQueueAsync(
		string queueKey,
		CreateQueueLocalFieldRequest request,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateLocalFieldInQueueAsync(queueKey, request, cancellationToken);
	}

	public Task<IReadOnlyList<GetFieldCategoriesResponse>> GetFieldCategoriesAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_localFieldCategories";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetFieldCategoriesAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public Task<IReadOnlyList<GetIssueTypeResponse>> GetIssueTypesAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_issueTypes";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetIssueTypesAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public Task<IReadOnlyList<ProjectInfo>> GetProjectsAsync(
		ProjectEntityType entityType,
		GetProjectsRequest request,
		ProjectFieldData? returnedFields = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_projects_{entityType}_{request.GetHashCode()}_{returnedFields}_" +
			$"{request.Input}_{request.OrderBy}_{request.OrderAsc}_{request.RootOnly}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetProjectsAsync(
					entityType,
					request,
					returnedFields,
					paginationSettings,
					cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public Task<GetQueuesResponse> GetQueueAsync(
		string queueKey,
		QueueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_queues_{queueKey}_{expand}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetQueueAsync(queueKey, expand, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public Task<IReadOnlyList<GetQueuesResponse>> GetQueuesAsync(
		QueuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_queues_{expand}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetQueuesAsync(expand, paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public Task<IReadOnlyList<GetResolutionResponse>> GetResolutionsAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_resolutions";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetResolutionsAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public Task<IReadOnlyList<string>> GetTagsAsync(
		string queueKey,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_tags_{queueKey}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetTagsAsync(queueKey, paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public Task<UserDetailedInfoDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_users_{userId}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetUserByIdAsync(userId, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public Task<IReadOnlyList<UserDetailedInfoDto>> GetUsersAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_users";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetUsersAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public void Dispose()
	{
		yandexTrackerClient.Dispose();
	}
}