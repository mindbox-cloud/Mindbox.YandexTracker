using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mindbox.YandexTracker;
public interface IGitlabClient
{
	Task<QueueEntity> GetQueueAsync(
		string queue,
		Expand? expand = null);

	Task<IReadOnlyCollection<QueueEntity>> GetQueuesAsync(
		Expand? expand = null);

	Task<Issue> GetIssueAsync(
		string issue,
		Expand? expand = null);

	Task<IReadOnlyCollection<Issue>> GetIssuesAsync(
		GetIssuesRequest request,
		Expand? expand = null);

	Task<Issue> CreateIssueAsync(CreateIssueRequest dto);

	Task<IReadOnlyCollection<Component>> GetComponentsAsync();

	Task<IReadOnlyCollection<Comment>> GetCommentsAsync(
		string issue,
		Expand? expand = null);

	Task<Comment> CreateCommentAsync(
		CreateCommentRequest request,
		string issue,
		bool? isAddToFollowers = null);
}
