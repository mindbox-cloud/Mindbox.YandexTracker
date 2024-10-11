using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record Resolution
{
	[DataMember(Name = "id")]
	public int Id { get; init; }

	[DataMember(Name = "key")]
	public required string Key { get; init; }

	[DataMember(Name = "name")]
	public required string Name { get; init; }

	[DataMember(Name = "description")]
	public string Description { get; init; } = null!;
}
