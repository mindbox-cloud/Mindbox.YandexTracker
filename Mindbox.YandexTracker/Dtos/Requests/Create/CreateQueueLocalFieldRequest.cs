using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record CreateQueueLocalFieldRequest
{
	[DataMember(Name = "id")]
	public required string Id { get; init; }

	[DataMember(Name = "name")]
	public required QueueLocalFieldName Name { get; init; }

	[DataMember(Name = "category")]
	public required string CategoryId { get; init; }

	[DataMember(Name = "type")]
	[JsonConverter(typeof(StringEnumConverter))]
	public QueueLocalFieldType Type { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "optionsProvider")]
	public OptionsProviderInfoDto? OptionsProvider { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "order")]
	public int? Order { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "description")]
	public string? Description { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "readonly")]
	public bool? Readonly { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "visible")]
	public bool? Visible { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "hidden")]
	public bool? Hidden { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "container")]
	public bool? Container { get; init; }
}