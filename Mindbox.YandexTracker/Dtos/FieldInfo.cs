namespace Mindbox.YandexTracker;

internal sealed record FieldInfo
{
	public required string Id { get; init; }

	public string? Key { get; init; }

	public required string Display { get; init; }
}
