using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mindbox.YandexTracker;
public interface IGitlabClient
{
	Task<IReadOnlyCollection<Issue>> GetIssuesAsync(string repositoryName);
}
