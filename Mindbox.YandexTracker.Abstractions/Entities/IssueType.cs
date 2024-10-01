namespace Mindbox.YandexTracker;

public sealed record IssueType
{
	public required string Name { get; set; }
	public required string Key { get; set; }
	public string? Description { get; set; }
}