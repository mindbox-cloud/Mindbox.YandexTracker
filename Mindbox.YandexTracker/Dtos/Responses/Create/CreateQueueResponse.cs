namespace Mindbox.YandexTracker;

internal sealed record CreateQueueResponse
{
	public required long Id { get; init; }

	public required string Key { get; init; }

	public required string Name { get; init; }

	public required FieldInfo Lead { get; init; }

	public bool AssignAuto { get; init; }

	public bool AllowExternals { get; init; }

	public required FieldInfo DefaultType { get; init; }

	public required FieldInfo DefaultPriority { get; init; }
}
