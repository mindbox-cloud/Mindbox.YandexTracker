namespace Mindbox.YandexTracker;

public record UserShortInfoDto
{
	public required string Id { get; init; }

	public string? PassportUid { get; init; }

	// TODO Этого поля где-то нет. Почему?
	public string? CloudUid { get; init; }

	public required string Display { get; init; }
}