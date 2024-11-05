namespace Mindbox.YandexTracker;

/// <summary>
/// Резолюция — это атрибут задачи, который обозначает причину ее закрытия
/// </summary>
public sealed record GetResolutionResponse
{
	/// <summary>
	/// Идентификатор
	/// </summary>
	public int Id { get; init; }

	/// <summary>
	/// Ключ
	/// </summary>
	public required string Key { get; init; }

	/// <summary>
	/// Название
	/// </summary>
	public required string Name { get; init; }

	/// <summary>
	/// Описание
	/// </summary>
	public string Description { get; init; } = null!;
}
