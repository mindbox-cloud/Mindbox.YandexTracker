using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record GetQueuesResponse
{
	public required long Id { get; init; }

	public required int Version { get; init; }

	public required string Key { get; init; }

	public required string Name { get; init; }

	public string? Description { get; init; }

	public required FieldInfo Lead { get; init; }

	public bool AssignAuto { get; init; }

	public required FieldInfo DefaultType { get; init; }

	public required FieldInfo DefaultPriority { get; init; }

	public Collection<FieldInfo> TeamUsers { get; init; } = [];

	public Collection<FieldInfo> IssueTypes { get; init; } = [];

	public Collection<FieldInfo> Versions { get; init; } = [];

	public Dictionary<string, Collection<FieldInfo>> Workflows { get; init; } = [];

	public Collection<IssueTypeConfigDto> IssueTypesConfig { get; init; } = [];

	public bool DenyVoting { get; init; }
}