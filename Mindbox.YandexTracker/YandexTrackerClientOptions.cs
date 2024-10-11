namespace Mindbox.YandexTracker;

public sealed record YandexTrackerClientOptions
{
	public required string OAuthToken { get; init; }
	public required string Organization { get; init; }
}
