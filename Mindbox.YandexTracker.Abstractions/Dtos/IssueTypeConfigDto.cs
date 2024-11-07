using System.Collections.Generic;

namespace Mindbox.YandexTracker;

public sealed record IssueTypeConfigDto
{
	public required FieldInfo IssueType { get; init; }

	public required FieldInfo Workflow { get; init; }

	public required IReadOnlyCollection<FieldInfo> Resolutions { get; init; } = [];
}
