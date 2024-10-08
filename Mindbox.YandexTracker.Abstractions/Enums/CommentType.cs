using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public enum CommentType
{
	[EnumMember(Value = "standard")]
	Standard,

	[EnumMember(Value = "incoming")]
	Incoming,

	[EnumMember(Value = "outcoming")]
	Outcoming
}
