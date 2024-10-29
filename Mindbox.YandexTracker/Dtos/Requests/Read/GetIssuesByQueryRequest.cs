namespace Mindbox.YandexTracker;

internal sealed record GetIssuesByQueryRequest
{
	public required string Query { get; init; }
}
