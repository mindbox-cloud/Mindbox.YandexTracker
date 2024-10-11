using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record CreateComponentRequest
{
	[DataMember(Name = "name")]
	public required string Name { get; init; }

	[DataMember(Name = "queue")]
	public required string Queue { get; init; }

	[DataMember(Name = "description")]
	public string? Description { get; init; }

	[DataMember(Name = "lead")]
	public string? Lead { get; init; }

	[DataMember(Name = "assignAuto")]
	public bool? AssignAuto { get; init; }
}
