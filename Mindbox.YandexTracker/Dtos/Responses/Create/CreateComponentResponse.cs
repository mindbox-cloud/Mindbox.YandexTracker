using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record CreateComponentResponse
{
	[DataMember(Name = "id")]
	public int Id { get; init; }

	[DataMember(Name = "name")]
	public string Name { get; init; } = null!;

	[DataMember(Name = "queue")]
	public FieldInfo Queue { get; init; } = null!;

	[DataMember(Name = "assignAuto")]
	public bool AssignAuto { get; init; }
}
