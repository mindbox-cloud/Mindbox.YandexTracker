namespace Mindbox.YandexTracker;

public sealed record GetComponentResponse
{
	public int Id { get; init; }

	public required string Name { get; init; }

	public required FieldInfo Queue { get; init; }

	public string? Description { get; init; }

	public FieldInfo? Lead { get; init; }

	public bool AssignAuto { get; init; }
}
