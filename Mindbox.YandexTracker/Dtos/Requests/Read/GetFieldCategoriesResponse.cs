using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public record GetFieldCategoriesResponse
{
	[DataMember(Name = "id")]
	public required string Id { get; init; }

	[DataMember(Name = "name")]
	public required string Name { get; init; }
}