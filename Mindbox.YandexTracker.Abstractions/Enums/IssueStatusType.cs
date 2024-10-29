namespace Mindbox.YandexTracker;

/// <summary>
/// Тип статуса задачи
/// </summary>
public enum IssueStatusType
{
	/// <summary>
	/// Начальный
	/// </summary>
	New,

	/// <summary>
	/// На паузе
	/// </summary>
	Paused,

	/// <summary>
	/// В процессе
	/// </summary>
	InProgress,

	/// <summary>
	/// Завершен
	/// </summary>
	Done,

	/// <summary>
	/// Отменен
	/// </summary>
	Cancelled
}