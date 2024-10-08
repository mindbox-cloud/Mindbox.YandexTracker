using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public enum Priority
{
	[EnumMember(Value = "trivial")]
	Trivial,

	[EnumMember(Value = "minor")]
	Minor,

	[EnumMember(Value = "normal")]
	Normal,

	[EnumMember(Value = "critical")]
	Critical,

	[EnumMember(Value = "blocker")]
	Blocker
}
