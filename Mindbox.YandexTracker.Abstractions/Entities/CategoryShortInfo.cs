namespace Mindbox.YandexTracker;

/// <summary>
/// Информация о категории поля
/// </summary>
public sealed record CategoryShortInfo
{
	/// <summary>
	/// Идентификатор категории поля
	/// </summary>
	public required string Id { get; init; }

	/// <summary>
	/// Название категории
	/// </summary>
	public required string Name { get; init; }
}