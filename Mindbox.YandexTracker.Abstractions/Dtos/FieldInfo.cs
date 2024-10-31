namespace Mindbox.YandexTracker;

public sealed record FieldInfo
{
	public required string Id { get; init; }

	public string? Key { get; init; }

	public required string Display { get; init; }
}
