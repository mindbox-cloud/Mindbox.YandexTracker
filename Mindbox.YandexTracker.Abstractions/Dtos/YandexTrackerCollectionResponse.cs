using System.Collections.Generic;

namespace Mindbox.YandexTracker;

public record YandexTrackerCollectionResponse<TItem>
{
	public int StartPage { get; init; }
	public int FetchedPages { get; init; }
	public int TotalPages { get; init; }
	public IReadOnlyList<TItem> Values { get; init; } = [];
}