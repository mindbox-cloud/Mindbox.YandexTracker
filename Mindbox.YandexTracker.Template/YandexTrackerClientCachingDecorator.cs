using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Mindbox.YandexTracker.Template;

public sealed class YandexTrackerClientCachingDecorator(
	IYandexTrackerClient yandexTrackerClient,
	IMemoryCache cache,
	YandexTrackerClientCachingDecoratorOptions options) : IYandexTrackerClient
{
	public Task<Attachment> CreateAttachmentAsync(
		string issueKey,
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateAttachmentAsync(issueKey, fileStream, newFileName, cancellationToken);
	}

	public Task<Comment> CreateCommentAsync(
		string issueKey,
		Comment comment,
		bool? isAddToFollowers = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateCommentAsync(issueKey, comment, isAddToFollowers, cancellationToken);
	}

	public Task<Issue> CreateIssueAsync(Issue issue, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateIssueAsync(issue, cancellationToken);
	}

	public Task<Project> CreateProjectAsync(
		ProjectEntityType entityType,
		Project project,
		ProjectFieldData? fields = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateProjectAsync(entityType, project, fields, cancellationToken);
	}

	public Task<Queue> CreateQueueAsync(Queue queue, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateQueueAsync(queue, cancellationToken);
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
		bool? withBoard = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.DeleteProjectAsync(entityType, projectShortId, withBoard, cancellationToken);
	}

	public Task DeleteQueueAsync(string queueKey, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.DeleteQueueAsync(queueKey, cancellationToken);
	}

	public Task<IReadOnlyList<IssueField>> GetAccessibleFieldsForIssueAsync(
		string queueKey,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CacheKeyPrefix}_accessibleFields";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetAccessibleFieldsForIssueAsync(queueKey, cancellationToken);

				entry.SetAbsoluteExpiration(options.TTLInMinutes);

				return result;
			})!;
	}

	public Task<IReadOnlyList<Attachment>> GetAttachmentsAsync(string issueKey, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetAttachmentsAsync(issueKey, cancellationToken);
	}

	public Task<IReadOnlyList<Comment>> GetCommentsAsync(
		string issueKey,
		CommentExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetCommentsAsync(issueKey, expand, cancellationToken);
	}

	public Task<IReadOnlyList<Component>> GetComponentsAsync(CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CacheKeyPrefix}_components";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetComponentsAsync(cancellationToken);

				entry.SetAbsoluteExpiration(options.TTLInMinutes);

				return result;
			})!;
	}

	public Task<Issue> GetIssueAsync(
		string issueKey,
		IssueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetIssueAsync(issueKey, expand, cancellationToken);
	}

	public Task<IReadOnlyList<Issue>> GetIssuesByFilterAsync(
		IssuesFilter issuesFilter,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CacheKeyPrefix}_issues_{issuesFilter.GetHashCode()}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetIssuesByFilterAsync(issuesFilter, expand, cancellationToken);

				entry.SetAbsoluteExpiration(options.TTLInMinutes);

				return result;
			})!;
	}

	public Task<IReadOnlyList<Issue>> GetIssuesByQueryAsync(
		string query,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CacheKeyPrefix}_issues_{query}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetIssuesByQueryAsync(query, expand, cancellationToken);

				entry.SetAbsoluteExpiration(options.TTLInMinutes);

				return result;
			})!;
	}

	public Task<IReadOnlyList<Issue>> GetIssuesFromKeysAsync(
		IReadOnlyList<string> keys,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CacheKeyPrefix}_issues_{keys.GetCollectionHashCode()}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetIssuesFromKeysAsync(keys, expand, cancellationToken);

				entry.SetAbsoluteExpiration(options.TTLInMinutes);

				return result;
			})!;
	}

	public Task<IReadOnlyList<Issue>> GetIssuesFromQueueAsync(
		string queueKey,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CacheKeyPrefix}_issues_{queueKey}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetIssuesFromQueueAsync(queueKey, expand, cancellationToken);

				entry.SetAbsoluteExpiration(options.TTLInMinutes);

				return result;
			})!;
	}

	public Task<IReadOnlyList<IssueStatus>> GetIssueStatusesAsync(CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CacheKeyPrefix}_issueStatuses";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetIssueStatusesAsync(cancellationToken);

				entry.SetAbsoluteExpiration(options.TTLInMinutes);

				return result;
			})!;
	}

	public Task<IReadOnlyList<IssueType>> GetIssueTypesAsync(CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CacheKeyPrefix}_issueTypes";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetIssueTypesAsync(cancellationToken);

				entry.SetAbsoluteExpiration(options.TTLInMinutes);

				return result;
			})!;
	}

	public Task<IReadOnlyList<Project>> GetProjectsAsync(
		ProjectEntityType entityType,
		Project project,
		ProjectFieldData? fields = null,
		string? input = null,
		string? orderBy = null,
		bool? orderAscending = null,
		bool? rootOnly = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CacheKeyPrefix}_projects_{entityType}_{project.GetHashCode()}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetProjectsAsync(
					entityType,
					project,
					fields,
					input,
					orderBy,
					orderAscending,
					rootOnly,
					cancellationToken);

				entry.SetAbsoluteExpiration(options.TTLInMinutes);

				return result;
			})!;
	}

	public Task<Queue> GetQueueAsync(
		string queueKey,
		QueueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetQueueAsync(queueKey, expand, cancellationToken);
	}

	public Task<IReadOnlyList<Queue>> GetQueuesAsync(
		QueuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CacheKeyPrefix}_queues";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetQueuesAsync(expand, cancellationToken);

				entry.SetAbsoluteExpiration(options.TTLInMinutes);

				return result;
			})!;
	}

	public Task<IReadOnlyList<Resolution>> GetResolutionsAsync(CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CacheKeyPrefix}_resolutions";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetResolutionsAsync(cancellationToken);

				entry.SetAbsoluteExpiration(options.TTLInMinutes);

				return result;
			})!;
	}

	public Task<IReadOnlyList<string>> GetTagsAsync(string queueKey, CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CacheKeyPrefix}_tags_{queueKey}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetTagsAsync(queueKey, cancellationToken);

				entry.SetAbsoluteExpiration(options.TTLInMinutes);

				return result;
			})!;
	}

	public Task<UserDetailedInfo> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetUserByIdAsync(userId, cancellationToken);
	}

	public Task<IReadOnlyList<UserDetailedInfo>> GetUsersAsync(CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CacheKeyPrefix}_users";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetUsersAsync(cancellationToken);

				entry.SetAbsoluteExpiration(options.TTLInMinutes);

				return result;
			})!;
	}

	public void Dispose()
	{
		yandexTrackerClient.Dispose();
	}
}
