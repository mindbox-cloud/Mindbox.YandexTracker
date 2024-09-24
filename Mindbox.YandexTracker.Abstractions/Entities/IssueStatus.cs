namespace Mindbox.YandexTracker;

public class IssueStatus
{
	public required string Name { get; set; }
	public required IssueStatusType Type { get; set; }
	public required string Key { get; set; }
}