using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record GetIssueFieldsResponse
{
	[DataMember(Name = "id")]
	public required string Id { get; init; }

	[DataMember(Name = "key")]
	public required string Key { get; init; }

	[DataMember(Name = "name")]
	public required string Name { get; init; }

	[DataMember(Name = "readonly")]
	public bool Readonly { get; init; }

	[DataMember(Name = "options")]
	public bool Options { get; init; }

	[DataMember(Name = "suggest")]
	public bool Suggest { get; init; }

	[DataMember(Name = "suggestProvider")]
	public ProviderInfoDto? SuggestProvider { get; init; }

	[DataMember(Name = "optionsProvider")]
	public required OptionsProviderInfoDto OptionsProvider { get; init; }

	[DataMember(Name = "queryProvider")]
	public ProviderInfoDto? QueryProvider { get; init; }

	[DataMember(Name = "order")]
	public int Order { get; init; }

	[DataMember(Name = "category")]
	public required FieldInfo Category { get; init; }

	[DataMember(Name = "type")]
	public required string Type { get; init; }

	[DataMember(Name = "schema")]
	public required SchemaInfoDto Schema { get; init; }
}

[DataContract]
public class ProviderInfoDto
{
	[DataMember(Name = "type")]
	public required string Type { get; init; }
}

[DataContract]
public class SchemaInfoDto
{
	[DataMember(Name = "type")]
	public required string Type { get; init; }

	[DataMember(Name = "required")]
	public bool Required { get; init; }
}

[DataContract]
public class OptionsProviderInfoDto
{
	[DataMember(Name = "type")]
	public required string Type { get; init; }

	[DataMember(Name = "values")]
	public Collection<string> Values { get; init; } = [];
}