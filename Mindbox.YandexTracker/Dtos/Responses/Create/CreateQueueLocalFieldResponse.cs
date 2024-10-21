using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record CreateQueueLocalFieldResponse
{
	[DataMember(Name = "id")]
	public required string Id { get; init; }

	[DataMember(Name = "type")]
	public required string Type { get; init; }

	[DataMember(Name = "name")]
	public required string Name { get; init; }

	[DataMember(Name = "description")]
	public required string Description { get; init; }

	[DataMember(Name = "key")]
	public required string Key { get; init; }

	[DataMember(Name = "schema")]
	public required SchemaInfoDto Schema { get; init; }

	[DataMember(Name = "readonly")]
	public bool Readonly { get; init; }

	[DataMember(Name = "options")]
	public bool Options { get; init; }

	[DataMember(Name = "suggest")]
	public bool Suggest { get; init; }

	[DataMember(Name = "order")]
	public int Order { get; init; }

	[DataMember(Name = "category")]
	public required FieldInfo Category { get; init; }

	[DataMember(Name = "queryProvider")]
	public required ProviderInfoDto QueryProvider { get; init; }
}