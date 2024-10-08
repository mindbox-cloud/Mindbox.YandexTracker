using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record CreateQueueResponse
{
	[DataMember(Name = "id")]
	public required string Id { get; init; }

	[DataMember(Name = "key")]
	public required string Key { get; init; }

	[DataMember(Name = "name")]
	public required string Name { get; init; }

	[DataMember(Name = "lead")]
	public required FieldInfo Lead { get; init; }

	[DataMember(Name = "assignAuto")]
	public bool AssignAuto { get; init; }

	[DataMember(Name = "allowExternals")]
	public bool AllowExternals { get; init; }

	[DataMember(Name = "defaultType")]
	public required FieldInfo DefaultType { get; init; }

	[DataMember(Name = "defaultPriority")]
	public required FieldInfo DefaultPriority { get; init; }
}
