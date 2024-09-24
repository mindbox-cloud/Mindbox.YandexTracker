using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Статус проекта/портфеля
/// </summary>
[DataContract]
public enum ProjectEntityStatus
{
	[EnumMember(Value = "draft")]
	Draft,

	[EnumMember(Value = "in_progress")]
	InProgress,

	[EnumMember(Value = "launched")]
	Launched,

	[EnumMember(Value = "postponed")]
	Postponed,

	[EnumMember(Value = "at_risk")]
	AtRisk,

	[EnumMember(Value = "blocked")]
	Blocked,

	[EnumMember(Value = "according_to_plan")]
	AccordingToPlan
}
