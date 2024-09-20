using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;
public class IssueTypeConfig
{
	public IssueType IssueType { get; set; }
	public required string Workflow { get; set; }
	public Collection<Resolution> Resolutions { get; set; } = [];
}
