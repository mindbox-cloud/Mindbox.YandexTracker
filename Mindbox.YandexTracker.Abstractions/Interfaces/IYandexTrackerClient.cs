using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mindbox.YandexTracker;

public interface IYandexTrackerClient
{
	Task<Queue> GetQueueAsync(
		string queue,
		CancellationToken token,
		QueueExpandData? expand = null);

	Task<IReadOnlyCollection<Queue>> GetQueuesAsync(
		CancellationToken token,
		QueuesExpandData? expand = null);

	Task<Issue> GetIssueAsync(
		string issue,
		CancellationToken token,
		IssueExpandData? expand = null);

	Task<IReadOnlyCollection<Issue>> GetIssuesAsync(
		GetIssuesRequest request,
		CancellationToken token,
		IssueExpandData? expand = null);

	Task<Issue> CreateIssueAsync(CreateIssueRequest request, CancellationToken token);

	Task<IReadOnlyCollection<Component>> GetComponentsAsync(CancellationToken token);

	Task<IReadOnlyCollection<Comment>> GetCommentsAsync(
		string issue,
		CancellationToken token,
		CommentExpandData? expand = null);

	Task<Comment> CreateCommentAsync(
		CreateCommentRequest request,
		string issue,
		CancellationToken token,
		bool? isAddToFollowers = null);

	Task<IReadOnlyCollection<Attachment>> GetAttachmentsAsync(string issue, CancellationToken token);

	Task<Attachment> CreateAttachmentAsync(
		byte[] file,
		string issue,
		CancellationToken token,
		string? newFileName = null);

	Task<IReadOnlyCollection<string>> GetTagsAsync(string queue, CancellationToken token);

	Task<Project> CreateProjectAsync(
		CreateProjectRequest request,
		EntityType entityType,
		CancellationToken token);

	Task<IReadOnlyCollection<Project>> GetProjectsAsync(
		GetProjectsRequest request,
		EntityType entityType,
		CancellationToken token);

	Task<IReadOnlyCollection<IssueField>> GetIssueFieldsAsync(string field, CancellationToken token);
	Task<IssueField> CreateIssueFieldAsync(CreateIssueFieldRequest request, CancellationToken token);
}
