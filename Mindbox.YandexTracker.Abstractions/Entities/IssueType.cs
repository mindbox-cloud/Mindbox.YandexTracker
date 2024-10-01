namespace Mindbox.YandexTracker;

public sealed record IssueType
{
	public required string Name { get; init; }
	public required string Key { get; init; }
	public string? Description { get; init; }
}