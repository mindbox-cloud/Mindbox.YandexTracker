namespace Mindbox.YandexTracker;

internal sealed record CreateComponentRequest
{
	public required string Name { get; init; }

	public required string Queue { get; init; }

	public string? Description { get; init; }

	public string? Lead { get; init; }

	public bool? AssignAuto { get; init; }
}
