namespace Mindbox.YandexTracker;

public record GetFieldCategoriesResponse
{
	public required string Id { get; init; }

	public required string Name { get; init; }
}