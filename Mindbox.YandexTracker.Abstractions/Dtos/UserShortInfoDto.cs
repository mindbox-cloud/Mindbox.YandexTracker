namespace Mindbox.YandexTracker;

public record UserShortInfoDto
{
	public required string Id { get; init; }

	public long? PassportUid { get; init; }

	public string? CloudUid { get; init; }

	public required string Display { get; init; }
}