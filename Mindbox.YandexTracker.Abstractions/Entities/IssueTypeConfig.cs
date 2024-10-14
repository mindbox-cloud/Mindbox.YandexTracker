using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

/// <summary>
/// Настройки задачи очереди
/// </summary>
public sealed record IssueTypeConfig
{
	/// <summary>
	/// Тип задач
	/// </summary>
	public required IssueType IssueType { get; init; }

	/// <summary>
	/// Информация о жизненном цикле типа задачи
	/// </summary>
	public required string Workflow { get; init; }

	/// <summary>
	/// Массив с возможными резолюциями типа задачи
	/// </summary>
	public Collection<Resolution> Resolutions { get; init; } = [];
}
