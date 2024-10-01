namespace Mindbox.YandexTracker;

public sealed record Resolution
{
	public required string Name { get; set; }
	public required string Key { get; set; }
	public string? Description { get; set; }
}
