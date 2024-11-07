namespace Mindbox.YandexTracker;

public sealed record CreateComponentResponse
{
	public int Id { get; init; }

	public int Version { get; init; }

	public string Name { get; init; } = null!;

	public FieldInfo Queue { get; init; } = null!;

	public bool AssignAuto { get; init; }
}
