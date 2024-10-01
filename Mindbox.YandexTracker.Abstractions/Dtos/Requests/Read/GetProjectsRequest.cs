using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record GetProjectsRequest
{
	[DataMember(EmitDefaultValue = false, Name = "input")]
	public string? Input { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "filter")]
	public ProjectFieldsDto? Filter { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "orderBy")]
	public string? OrderBy { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "orderAsc")]
	public bool? OrderAscending { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "rootOnly")]
	public bool? RootOnly { get; init; }

	[JsonIgnore]
	public ProjectFieldData? FieldsWhichIncludedInResponse { get; init; }
}
