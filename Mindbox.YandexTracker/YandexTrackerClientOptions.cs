namespace Mindbox.YandexTracker;

public sealed record YandexTrackerClientOptions
{
	public PaginationSettings? DefaultPaginationSettings { get; init; }
	public required string OAuthToken { get; set; }
	public required string Organization { get; set; }
}
