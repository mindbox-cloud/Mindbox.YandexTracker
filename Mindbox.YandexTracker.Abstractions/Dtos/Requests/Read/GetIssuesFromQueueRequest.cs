namespace Mindbox.YandexTracker;

public sealed record GetIssuesFromQueueRequest
{
	public required string Queue { get; init; }
}
