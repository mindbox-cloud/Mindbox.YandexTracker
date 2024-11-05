namespace Mindbox.YandexTracker;

public record PaginationSettings
{
	public int PerPage { get; set; } = 100;
	public int? MaxPageRequestCount { get; set; }
}