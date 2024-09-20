using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed class FieldInfo
{
	[DataMember(Name = "id")]
	public int Id { get; set; }

	[DataMember(Name = "key")]
	public string? Key { get; set; }

	[DataMember(Name = "display")]
	public required string Display { get; set; }
}
