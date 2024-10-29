using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Статус проекта/портфеля
/// </summary>
public enum ProjectEntityStatus
{
	/// <summary>
	/// Черновик
	/// </summary>
	[EnumMember(Value = "draft")]
	Draft,

	/// <summary>
	/// В работе
	/// </summary>
	[EnumMember(Value = "in_progress")]
	InProgress,

	/// <summary>
	/// Новый
	/// </summary>
	[EnumMember(Value = "launched")]
	Launched,

	/// <summary>
	/// Отложен
	/// </summary>
	[EnumMember(Value = "postponed")]
	Postponed,

	/// <summary>
	/// Есть риски
	/// </summary>
	[EnumMember(Value = "at_risk")]
	AtRisk,

	/// <summary>
	/// Заблокирован
	/// </summary>
	[EnumMember(Value = "blocked")]
	Blocked,

	/// <summary>
	/// По плану
	/// </summary>
	[EnumMember(Value = "according_to_plan")]
	AccordingToPlan
}
