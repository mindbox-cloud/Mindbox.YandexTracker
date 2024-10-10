using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record FieldInfo
{
	[DataMember(Name = "id")]
	public required string Id { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "key")]
	public string? Key { get; init; }

	[DataMember(Name = "display")]
	public required string Display { get; init; }
}
