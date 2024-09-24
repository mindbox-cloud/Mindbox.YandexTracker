using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class GetComponentResponse
{
	[DataMember(Name = "id")]
	public int Id { get; set; }

	[DataMember(Name = "name")]
	public required string Name { get; set; }

	[DataMember(Name = "queue")]
	public required FieldInfo Queue { get; set; }

	[DataMember(Name = "description")]
	public string? Description { get; set; }

	[DataMember(Name = "lead")]
	public FieldInfo? Lead { get; set; }

	[DataMember(Name = "assignAuto")]
	public bool AssignAuto { get; set; }
}
