using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public enum FileType
{
	[EnumMember(Value = "text/plain")]
	TxtFile,

	[EnumMember(Value = "image/png")]
	ImageFile
}
