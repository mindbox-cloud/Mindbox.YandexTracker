using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record CreateQueueRequest
{
	public required string Key { get; init; }

	public required string Name { get; init; }

	public required string Lead { get; init; }

	public required string DefaultType { get; init; }

	public Priority DefaultPriority { get; init; }

	public Collection<CreateIssueTypeConfigDto> IssueTypesConfig { get; init; } = [];
}

public sealed record CreateIssueTypeConfigDto
{
	public required string IssueType { get; init; }

	public required string Workflow { get; init; }

	public Collection<string> Resolutions { get; init; } = [];
}