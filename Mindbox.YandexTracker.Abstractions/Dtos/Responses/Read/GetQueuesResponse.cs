using System.Collections.Generic;
using System.Collections.Immutable;

namespace Mindbox.YandexTracker;

public sealed record GetQueuesResponse
{
	public required long Id { get; init; }

	public required int Version { get; init; }

	public required string Key { get; init; }

	public required string Name { get; init; }

	public string? Description { get; init; }

	public required UserShortInfoDto Lead { get; init; }

	public bool AssignAuto { get; init; }

	public required FieldInfo DefaultType { get; init; }

	public required FieldInfo DefaultPriority { get; init; }

	public IReadOnlyCollection<UserShortInfoDto> TeamUsers { get; init; } = [];

	public IReadOnlyCollection<FieldInfo> IssueTypes { get; init; } = [];

	public IReadOnlyCollection<FieldInfo> Versions { get; init; } = [];

	public IReadOnlyDictionary<string, IReadOnlyCollection<FieldInfo>> Workflows { get; init; }
		= ImmutableDictionary<string, IReadOnlyCollection<FieldInfo>>.Empty;

	public IReadOnlyCollection<IssueTypeConfigDto> IssueTypesConfig { get; init; } = [];

	public bool DenyVoting { get; init; }
}