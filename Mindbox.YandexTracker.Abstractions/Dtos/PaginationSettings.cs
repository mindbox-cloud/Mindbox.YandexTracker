namespace Mindbox.YandexTracker;

public record PaginationSettings
{
	public int StartPage { get; init; } = 1;
	public int PerPage { get; set; } = 100;
	public int? MaxPageRequestCount { get; set; }
}