using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mindbox.YandexTracker;

public interface IYandexTrackerClient
{
	Task<Queue> GetQueueAsync(
		string queueKey,
		QueueExpandData? expand = null,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Queue>> GetQueuesAsync(
		QueuesExpandData? expand = null,
		CancellationToken cancellationToken = default);

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

	Task<IReadOnlyList<Attachment>> GetAttachmentsAsync(string issueKey, CancellationToken cancellationToken = default);

	Task<Attachment> CreateAttachmentAsync(
		string issueKey,
		byte[] file,
		string? newFileName = null,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<string>> GetTagsAsync(string queueKey, CancellationToken cancellationToken = default);

	Task<Project> CreateProjectAsync(
		ProjectEntityType entityType,
		CreateProjectRequest request,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Project>> GetProjectsAsync(
		ProjectEntityType entityType,
		GetProjectsRequest request,
		CancellationToken cancellationToken = default);

	Task<IssueField> GetAccessibleFieldsForIssueAsync(
		string queueKey,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<GetUserResponse>> GetUsersAsync(CancellationToken cancellationToken = default);
}
