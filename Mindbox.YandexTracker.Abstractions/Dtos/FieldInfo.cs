using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record FieldInfo
{
	[DataMember(Name = "id")]
	public int Id { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "key")]
	public string? Key { get; init; }

	[DataMember(Name = "display")]
	public required string Display { get; init; }
}
