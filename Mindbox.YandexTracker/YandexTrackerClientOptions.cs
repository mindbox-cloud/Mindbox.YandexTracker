namespace Mindbox.YandexTracker;

public sealed record YandexTrackerClientOptions
{
	public required string OAuthToken { get; set; }
	public required string Organization { get; set; }
}
