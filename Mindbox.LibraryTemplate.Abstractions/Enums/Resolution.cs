using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public enum Resolution
{
	[EnumMember(Value = "fixed")]
	Fixed,

	[EnumMember(Value = "wontFix")]
	WontFix,

	[EnumMember(Value = "cantReproduce")]
	CantReproduce,

	[EnumMember(Value = "duplicate")]
	Duplicate,

	[EnumMember(Value = "later")]
	Later,

	[EnumMember(Value = "overfulfilled")]
	Overfulfilled,

	[EnumMember(Value = "successful")]
	Successful,

	[EnumMember(Value = "dontDo")]
	DontDo
}
