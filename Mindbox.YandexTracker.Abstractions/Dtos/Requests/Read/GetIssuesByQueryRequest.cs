namespace Mindbox.YandexTracker;

public sealed record GetIssuesByQueryRequest
{
	public required string Query { get; init; }
}
