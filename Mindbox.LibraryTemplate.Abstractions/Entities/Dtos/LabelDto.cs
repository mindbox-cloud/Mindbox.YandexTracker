using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed class LabelDto
{
	[DataMember(Name = "name")]
	public required string Name { get; set; }
}