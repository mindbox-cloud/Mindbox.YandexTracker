namespace Mindbox.YandexTracker;

/// <summary>
/// Тип задачи
/// </summary>
public sealed record IssueType
{
	/// <summary>
	/// Идентификатор типа
	/// </summary>
	public int Id { get; init; }

	public int Version { get; init; }

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
