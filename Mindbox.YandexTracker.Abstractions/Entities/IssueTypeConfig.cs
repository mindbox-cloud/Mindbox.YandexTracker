using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record IssueTypeConfig
{
	public required IssueType IssueType { get; init; }
	public required string Workflow { get; init; }
	public Collection<Resolution> Resolutions { get; init; } = [];
}
