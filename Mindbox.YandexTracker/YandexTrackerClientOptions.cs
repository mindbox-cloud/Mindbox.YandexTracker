namespace Mindbox.YandexTracker;

public sealed record YandexTrackerClientOptions
{
	public required string Token { get; init; }
	public required string Organization { get; init; }
}
