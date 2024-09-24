using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class CreateIssueFieldResponse
{
	[DataMember(Name = "id")]
	public required string Id { get; set; }

	[DataMember(Name = "name")]
	public required string Name { get; set; }

	[DataMember(Name = "description")]
	public string? Description { get; set; }

	[DataMember(Name = "key")]
	public required string Key { get; set; }

	[DataMember(Name = "schema")]
	public required FieldSchemaDto Schema { get; set; }

	[DataMember(Name = "readonly")]
	public bool Readonly { get; set; }

	[DataMember(Name = "options")]
	public bool Options { get; set; }

	[DataMember(Name = "suggest")]
	public bool Suggest { get; set; }

	[DataMember(Name = "optionsProvider")]
	public OptionsProviderDto? OptionsProvider { get; set; }

	[DataMember(Name = "queryProvider")]
	public required ProviderDto QueryProvider { get; set; }

	[DataMember(Name = "order")]
	public int Order { get; set; }

	[DataMember(Name = "category")]
	public required FieldInfo Category { get; set; }

	[DataMember(Name = "type")]
	public required string Type { get; set; }

	[DataMember(Name = "suggestProvider")]
	public ProviderDto? SuggestProvider { get; set; }
}

[DataContract]
public class FieldSchemaDto
{
	[DataMember(Name = "type")]
	public required string Type { get; set; }

	[DataMember(Name = "items")]
	public required string Items { get; set; }

	[DataMember(Name = "required")]
	public bool Required { get; set; }
}

[DataContract]
public class ProviderDto
{
	[DataMember(Name = "type")]
	public required string Type { get; set; }
}