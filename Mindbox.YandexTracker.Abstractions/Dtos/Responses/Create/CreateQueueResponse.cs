namespace Mindbox.YandexTracker;

public sealed record CreateQueueResponse
{
	public required long Id { get; init; }

	public required string Key { get; init; }
	public required int Version { get; init; }

	public required string Name { get; init; }

	public required UserShortInfoDto Lead { get; init; }

	public bool AssignAuto { get; init; }

	public bool AllowExternals { get; init; }

	public required FieldInfo DefaultType { get; init; }

	public required FieldInfo DefaultPriority { get; init; }
}
