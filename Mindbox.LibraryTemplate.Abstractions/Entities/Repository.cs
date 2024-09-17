using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed class Repository
{
	[DataMember(Name = "name")]
	public required string Name { get; set; }
}