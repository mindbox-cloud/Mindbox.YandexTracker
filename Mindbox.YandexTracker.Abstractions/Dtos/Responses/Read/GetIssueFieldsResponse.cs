using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class GetIssueFieldsResponse
{
	[DataMember(Name = "id")]
	public required string Id { get; set; }

	[DataMember(Name = "key")]
	public required string Key { get; set; }

	[DataMember(Name = "name")]
	public required string Name { get; set; }

	[DataMember(Name = "readonly")]
	public bool Readonly { get; set; }

	[DataMember(Name = "options")]
	public bool Options { get; set; }

	[DataMember(Name = "suggest")]
	public bool Suggest { get; set; }

	[DataMember(Name = "suggestProvider")]
	public ProviderInfoDto? SuggestProvider { get; set; }

	[DataMember(Name = "optionsProvider")]
	public required OptionsProviderInfoDto OptionsProvider { get; set; }

	[DataMember(Name = "queryProvider")]
	public ProviderInfoDto? QueryProvider { get; set; }

	[DataMember(Name = "order")]
	public int Order { get; set; }

	[DataMember(Name = "category")]
	public required FieldInfo Category { get; set; }

	[DataMember(Name = "type")]
	public required string Type { get; set; }

	[DataMember(Name = "schema")]
	public required SchemaInfoDto Schema { get; set; }
}

[DataContract]
public class ProviderInfoDto
{
	[DataMember(Name = "type")]
	public required string Type { get; set; }
}

[DataContract]
public class SchemaInfoDto
{
	[DataMember(Name = "type")]
	public required string Type { get; set; }

	[DataMember(Name = "required")]
	public bool Required { get; set; }
}

[DataContract]
public class OptionsProviderInfoDto
{
	[DataMember(Name = "type")]
	public required string Type { get; set; }

	[DataMember(Name = "values")]
	public Collection<string> Values { get; set; } = [];
}