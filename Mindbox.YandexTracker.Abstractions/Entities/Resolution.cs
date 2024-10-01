namespace Mindbox.YandexTracker;

public sealed record Resolution
{
	public required string Name { get; init; }
	public required string Key { get; init; }
	public string? Description { get; init; }
}
