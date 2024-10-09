using Microsoft.Extensions.Caching.Memory;

namespace Mindbox.YandexTracker.Template;

public sealed class CachingYandexTrackerClient(
	IYandexTrackerClient yandexTrackerClient,
	IMemoryCache cache) : IYandexTrackerClient
{
	private readonly MemoryCacheEntryOptions _memoryCacheOptions =
		new MemoryCacheEntryOptions()
			.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

	public async Task<Attachment> CreateAttachmentAsync(
		string issueKey,
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default)
	{
		return await yandexTrackerClient.CreateAttachmentAsync(
			issueKey,
			fileStream,
			newFileName,
			cancellationToken);
	}

	public async Task<Comment> CreateCommentAsync(
		string issueKey,
		CreateCommentRequest request,
		CancellationToken cancellationToken = default)
	{
		return await yandexTrackerClient.CreateCommentAsync(
			issueKey,
			request,
			cancellationToken);
	}

	public async Task<Issue> CreateIssueAsync(CreateIssueRequest request, CancellationToken cancellationToken = default)
	{
		return await yandexTrackerClient.CreateIssueAsync(
			request,
			cancellationToken);
	}

	public async Task<Project> CreateProjectAsync(
		ProjectEntityType entityType,
		CreateProjectRequest request,
		CancellationToken cancellationToken = default)
	{
		return await yandexTrackerClient.CreateProjectAsync(entityType, request, cancellationToken);
	}

	public async Task<Queue> CreateQueueAsync(CreateQueueRequest request, CancellationToken cancellationToken = default)
	{
		return await yandexTrackerClient.CreateQueueAsync(
			request,
			cancellationToken);
	}

	public async Task DeleteAttachmentAsync(
		string issueKey,
		string attachmentKey,
		CancellationToken cancellationToken = default)
	{
		await yandexTrackerClient.DeleteAttachmentAsync(
			issueKey,
			attachmentKey,
			cancellationToken);
	}

	public async Task DeleteCommentAsync(
		string issueKey,
		int commentId,
		CancellationToken cancellationToken = default)
	{
		await yandexTrackerClient.DeleteCommentAsync(issueKey, commentId, cancellationToken);
	}

	public async Task DeleteProjectAsync(
		ProjectEntityType entityType,
		int projectShortId,
		CancellationToken cancellationToken = default)
	{
		await yandexTrackerClient.DeleteProjectAsync(
			entityType,
			projectShortId,
			cancellationToken);
	}

	public async Task DeleteQueueAsync(
		string queueKey,
		CancellationToken cancellationToken = default)
	{
		await yandexTrackerClient.DeleteQueueAsync(queueKey, cancellationToken);
	}

	public void Dispose()
	{
		yandexTrackerClient.Dispose();
	}

	public async Task<IReadOnlyList<IssueField>> GetAccessibleFieldsForIssueAsync(
		string queueKey,
		CancellationToken cancellationToken = default)
	{
		var accessibleFields = "accessibleFields";
		cache.TryGetValue(accessibleFields, out IReadOnlyList<IssueField>? cacheAccessibleFields);

		if (cacheAccessibleFields is not null)
			return cacheAccessibleFields;

		var accessibleFieldsFromTracker = await yandexTrackerClient.GetAccessibleFieldsForIssueAsync(queueKey, cancellationToken);

		cache.Set(accessibleFields, accessibleFieldsFromTracker, _memoryCacheOptions);

		return accessibleFieldsFromTracker;
	}

	public async Task<IReadOnlyList<Attachment>> GetAttachmentsAsync(
		string issueKey,
		CancellationToken cancellationToken = default)
	{
		return await yandexTrackerClient.GetAttachmentsAsync(issueKey, cancellationToken);
	}

	public async Task<IReadOnlyList<Comment>> GetCommentsAsync(
		string issueKey,
		CommentExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		return await yandexTrackerClient.GetCommentsAsync(issueKey, expand, cancellationToken);
	}

	public async Task<IReadOnlyList<Component>> GetComponentsAsync(CancellationToken cancellationToken = default)
	{
		var components = "components";
		cache.TryGetValue(components, out IReadOnlyList<Component>? cacheComponents);

		if (cacheComponents is not null)
			return cacheComponents;

		var componentsFromTracker = await yandexTrackerClient.GetComponentsAsync(cancellationToken);

		cache.Set(components, componentsFromTracker, _memoryCacheOptions);
		return componentsFromTracker;
	}

	public async Task<Issue> GetIssueAsync(
		string issueKey,
		IssueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		return await yandexTrackerClient.GetIssueAsync(issueKey, expand, cancellationToken);
	}

	public Task<IReadOnlyList<Issue>> GetIssuesAsync(GetIssuesRequest request, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<IReadOnlyList<IssueStatus>> GetIssueStatusesAsync(CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public async Task<IReadOnlyList<IssueType>> GetIssueTypesAsync(CancellationToken cancellationToken = default)
	{
		return await yandexTrackerClient.GetIssueTypesAsync(cancellationToken);
	}

	public async Task<IReadOnlyList<Project>> GetProjectsAsync(
		ProjectEntityType entityType,
		GetProjectsRequest request,
		CancellationToken cancellationToken = default)
	{
		var projects = "projects";
		cache.TryGetValue(projects, out IReadOnlyList<Project>? cacheProjects);

		if (cacheProjects is not null)
			return cacheProjects;

		var projectsFromTrackers = await yandexTrackerClient.GetProjectsAsync(
			entityType,
			request,
			cancellationToken);

		cache.Set(projects, projectsFromTrackers, _memoryCacheOptions);

		return projectsFromTrackers;
	}

	public async Task<Queue> GetQueueAsync(
		string queueKey,
		QueueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		return await yandexTrackerClient.GetQueueAsync(queueKey, expand, cancellationToken);
	}

	public async Task<IReadOnlyList<Queue>> GetQueuesAsync(
		QueuesExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		var queues = "queues";
		cache.TryGetValue(queues, out IReadOnlyList<Queue>? cacheQueues);

		if (cacheQueues is not null)
			return cacheQueues;

		var queuesFromTracker = await yandexTrackerClient.GetQueuesAsync(expand, cancellationToken);

		cache.Set(queues, queuesFromTracker, _memoryCacheOptions);

		return queuesFromTracker;
	}

	public async Task<IReadOnlyList<Resolution>> GetResolutionsAsync(CancellationToken cancellationToken = default)
	{
		return await yandexTrackerClient.GetResolutionsAsync(cancellationToken);
	}

	public async Task<IReadOnlyList<string>> GetTagsAsync(string queueKey, CancellationToken cancellationToken = default)
	{
		var tags = "tags";
		cache.TryGetValue(tags, out IReadOnlyList<string>? cacheTags);

		if (cacheTags is not null)
			return cacheTags;

		var tagsFromTracker = await yandexTrackerClient.GetTagsAsync(queueKey, cancellationToken);

		cache.Set(tags, tagsFromTracker, _memoryCacheOptions);

		return tagsFromTracker;
	}

	public async Task<UserDetailedInfo> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
	{
		return await yandexTrackerClient.GetUserByIdAsync(userId, cancellationToken);
	}

	public async Task<IReadOnlyList<UserDetailedInfo>> GetUsersAsync(CancellationToken cancellationToken = default)
	{
		return await yandexTrackerClient.GetUsersAsync(cancellationToken);
	}
}
