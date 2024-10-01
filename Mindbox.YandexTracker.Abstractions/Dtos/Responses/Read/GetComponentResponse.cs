using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record GetComponentResponse
{
	[DataMember(Name = "id")]
	public int Id { get; init; }

	[DataMember(Name = "name")]
	public required string Name { get; init; }

	[DataMember(Name = "queue")]
	public required FieldInfo Queue { get; init; }

	[DataMember(Name = "description")]
	public string? Description { get; init; }

	[DataMember(Name = "lead")]
	public FieldInfo? Lead { get; init; }

	[DataMember(Name = "assignAuto")]
	public bool AssignAuto { get; init; }
}
