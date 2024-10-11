namespace Mindbox.YandexTracker;

/// <summary>
/// Информация о пользователе
/// </summary>
public sealed record UserShortInfo
{
	/// <summary>
	/// Идентификатор пользователя
	/// </summary>
	public required string Id { get; init; }
}