using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public enum EntityStatus
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
