namespace Mindbox.YandexTracker;

public sealed record Component
{
	public int Id { get; init; }
	public required string Name { get; init; }
	public string? Queue { get; init; }
	public string? Description { get; init; }
	public required bool AssignAuto { get; init; }
	public UserShortInfo? Lead { get; init; }
}
