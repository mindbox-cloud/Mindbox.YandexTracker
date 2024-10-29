using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

internal sealed record IssueTypeConfigDto
{
	public required FieldInfo IssueType { get; init; }

	public required FieldInfo Workflow { get; init; }

	public required Collection<FieldInfo> Resolutions { get; init; }
}
