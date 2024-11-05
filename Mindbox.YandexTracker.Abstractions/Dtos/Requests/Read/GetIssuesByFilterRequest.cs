namespace Mindbox.YandexTracker;

public sealed record GetIssuesByFilterRequest
{
	public required IssuesFilterDto Filter { get; init; }

	public string? Order { get; init; }
}
