using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public enum CommentTransportType
{
	[EnumMember(Value = "internal")]
	Internal,

	[EnumMember(Value = "email")]
	Email
}
