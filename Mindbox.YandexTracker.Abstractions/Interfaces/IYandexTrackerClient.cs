using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mindbox.YandexTracker;

public interface IYandexTrackerClient : IDisposable
{
	Task<Queue> GetQueueAsync(
		string queueKey,
		QueueExpandData? expand = null,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Queue>> GetQueuesAsync(
		QueuesExpandData? expand = null,
		CancellationToken cancellationToken = default);

	Task<Queue> CreateQueueAsync(
		CreateQueueRequest request,
		CancellationToken cancellationToken = default);

	Task DeleteQueueAsync(string queueKey, CancellationToken cancellationToken = default);

	Task<Issue> GetIssueAsync(
		string issueKey,
		IssueExpandData? expand = null,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Issue>> GetIssuesAsync(
		GetIssuesRequest request,
		CancellationToken cancellationToken = default);

	Task<Issue> CreateIssueAsync(CreateIssueRequest request, CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Component>> GetComponentsAsync(CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Comment>> GetCommentsAsync(
		string issueKey,
		CommentExpandData? expand = null,
		CancellationToken cancellationToken = default);

	Task<Comment> CreateCommentAsync(
		string issueKey,
		CreateCommentRequest request,
		CancellationToken cancellationToken = default);

	Task DeleteCommentAsync(
		string issueKey,
		string commentKey,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Attachment>> GetAttachmentsAsync(string issueKey, CancellationToken cancellationToken = default);

	Task<Attachment> CreateAttachmentAsync(
		string issueKey,
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default);

	Task DeleteAttachmentAsync(
		string issueKey,
		string attachmentKey,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<string>> GetTagsAsync(string queueKey, CancellationToken cancellationToken = default);

	Task<Project> CreateProjectAsync(
		ProjectEntityType entityType,
		CreateProjectRequest request,
		CancellationToken cancellationToken = default);

	Task DeleteProjectAsync(
		ProjectEntityType entityType,
		string projectKey,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Project>> GetProjectsAsync(
		ProjectEntityType entityType,
		GetProjectsRequest request,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<IssueField>> GetAccessibleFieldsForIssueAsync(
		string queueKey,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<GetUserResponse>> GetUsersAsync(CancellationToken cancellationToken = default);

	Task<GetUserResponse> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);

	Task<IReadOnlyList<GetIssueTypeResponse>> GetIssueTypesAsync(
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<GetResolutionResponse>> GetResolutionsAsync(
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<GetIssueStatusResponse>> GetIssueStatusesAsync(
		CancellationToken cancellationToken = default);
}
