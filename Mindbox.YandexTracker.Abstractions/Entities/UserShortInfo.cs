namespace Mindbox.YandexTracker;

/// <summary>
/// Информация о пользователе
/// </summary>
public sealed record UserShortInfo
{
	/// <summary>
	/// Идентификатор пользователя
	/// </summary>
	public required long Id { get; init; }

	/// <summary>
	/// Отображаемое имя пользователя.
	/// </summary>
	public required string Display { get; init; }
}