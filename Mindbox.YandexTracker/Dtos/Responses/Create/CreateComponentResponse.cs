namespace Mindbox.YandexTracker;

internal sealed record CreateComponentResponse
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;

	public FieldInfo Queue { get; init; } = null!;

	public bool AssignAuto { get; init; }
}
