using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mindbox.YandexTracker;

public interface IYandexTrackerClient
{
	Task<Queue> GetQueueAsync(
		string queue,
		QueueExpandData? expand = null,
		CancellationToken token = default);

	Task<IReadOnlyCollection<Queue>> GetQueuesAsync(
		QueuesExpandData? expand = null,
		CancellationToken token = default);

	Task<Issue> GetIssueAsync(
		string issue,
		IssueExpandData? expand = null,
		CancellationToken token = default);

	Task<IReadOnlyCollection<Issue>> GetIssuesAsync(
		GetIssuesRequest request,
		IssueExpandData? expand = null,
		CancellationToken token = default);

	Task<Issue> CreateIssueAsync(CreateIssueRequest request, CancellationToken token = default);

	Task<IReadOnlyCollection<Component>> GetComponentsAsync(CancellationToken token = default);

	Task<IReadOnlyCollection<Comment>> GetCommentsAsync(
		string issue,
		CommentExpandData? expand = null,
		CancellationToken token = default);

	Task<Comment> CreateCommentAsync(
		CreateCommentRequest request,
		string issue,
		bool? isAddToFollowers = null,
		CancellationToken token = default);

	Task<IReadOnlyCollection<Attachment>> GetAttachmentsAsync(string issue, CancellationToken token = default);

	Task<Attachment> CreateAttachmentAsync(
		byte[] file,
		string issue,
		string? newFileName = null,
		CancellationToken token = default);

	Task<IReadOnlyCollection<string>> GetTagsAsync(string queue, CancellationToken token = default);

	Task<Project> CreateProjectAsync(
		CreateProjectRequest request,
		ProjectEntityType entityType,
		CancellationToken token = default);

	Task<IReadOnlyCollection<Project>> GetProjectsAsync(
		GetProjectsRequest request,
		ProjectEntityType entityType,
		CancellationToken token = default);

	Task<IReadOnlyCollection<IssueField>> GetIssueFieldsAsync(string field, CancellationToken token = default);
}
