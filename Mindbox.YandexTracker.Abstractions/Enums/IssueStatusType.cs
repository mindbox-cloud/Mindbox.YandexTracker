using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public enum IssueStatusType
{
	[EnumMember(Value = "new")]
	New,

	[EnumMember(Value = "paused")]
	Paused,

	[EnumMember(Value = "inProgress")]
	InProgress,

	[EnumMember(Value = "done")]
	Done,

	[EnumMember(Value = "cancelled")]
	Cancelled
}