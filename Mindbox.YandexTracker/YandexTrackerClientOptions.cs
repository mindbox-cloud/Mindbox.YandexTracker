namespace Mindbox.YandexTracker;

public sealed record YandexTrackerClientOptions
{
	public required string Token { get; set; }
	public required string Organization { get; set; }
}
