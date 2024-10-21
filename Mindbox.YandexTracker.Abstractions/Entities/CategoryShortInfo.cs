namespace Mindbox.YandexTracker;

/// <summary>
/// Информация о категории поля
/// </summary>
public sealed record CategoryShortInfo
{
	/// <summary>
	/// Идентификатор категории поля
	/// </summary>
	public string Id { get; init; } = string.Empty;

	/// <summary>
	/// Название категории
	/// </summary>
	public string Name { get; init; } = string.Empty;
}