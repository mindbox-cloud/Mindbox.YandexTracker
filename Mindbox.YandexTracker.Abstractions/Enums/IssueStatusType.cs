using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Тип статуса задачи
/// </summary>
[DataContract]
public enum IssueStatusType
{
	/// <summary>
	/// Начальный
	/// </summary>
	[EnumMember(Value = "new")]
	New,

	/// <summary>
	/// На паузе
	/// </summary>
	[EnumMember(Value = "paused")]
	Paused,

	/// <summary>
	/// В процессе
	/// </summary>
	[EnumMember(Value = "inProgress")]
	InProgress,

	/// <summary>
	/// Завершен
	/// </summary>
	[EnumMember(Value = "done")]
	Done,

	/// <summary>
	/// Отменен
	/// </summary>
	[EnumMember(Value = "cancelled")]
	Cancelled
}