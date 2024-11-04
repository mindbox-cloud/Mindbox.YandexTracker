namespace Mindbox.YandexTracker;

public sealed record GetComponentResponse
{
	public int Id { get; init; }

	public int Version { get; init; }

	public required string Name { get; init; }

	public required FieldInfo Queue { get; init; }

	public string? Description { get; init; }

	public UserShortInfoDto? Lead { get; init; }

	public bool AssignAuto { get; init; }
}