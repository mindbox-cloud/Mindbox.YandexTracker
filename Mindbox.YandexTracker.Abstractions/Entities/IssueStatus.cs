namespace Mindbox.YandexTracker;

public sealed record IssueStatus
{
	public required string Name { get; init; }
	public required IssueStatusType Type { get; init; }
	public required string Key { get; init; }
}