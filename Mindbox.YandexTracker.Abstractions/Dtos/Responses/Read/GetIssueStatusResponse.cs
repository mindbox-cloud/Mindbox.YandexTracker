namespace Mindbox.YandexTracker;

/// <summary>
/// Статус задачи
/// </summary>
public sealed record GetIssueStatusResponse
{
	/// <summary>
	/// Идентификатор статуса
	/// </summary>
	public int Id { get; init; }

	/// <summary>
	/// Ключ статуса
	/// </summary>
	public required string Key { get; init; }

	/// <summary>
	/// Название статуса
	/// </summary>
	public required string Name { get; init; }

	/// <summary>
	/// Описание статуса
	/// </summary>
	public string Description { get; init; } = null!;

	/// <summary>
	/// Тип статуса задачи
	/// </summary>
	public IssueStatusType Type { get; init; }
}
