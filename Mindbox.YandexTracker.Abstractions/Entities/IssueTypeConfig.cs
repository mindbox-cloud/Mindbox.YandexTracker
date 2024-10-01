using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record IssueTypeConfig
{
	public required IssueType IssueType { get; set; }
	public required string Workflow { get; set; }
	public Collection<Resolution> Resolutions { get; set; } = [];
}
