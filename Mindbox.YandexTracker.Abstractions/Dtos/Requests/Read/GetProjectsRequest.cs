using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class GetProjectsRequest
{
	[DataMember(EmitDefaultValue = false, Name = "input")]
	public string? Input { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "filter")]
	public ProjectFieldsDto? Filter { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "orderBy")]
	public string? OrderBy { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "orderAsc")]
	public bool? OrderAscending { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "rootOnly")]
	public bool? RootOnly { get; set; }

	[JsonIgnore]
	public ProjectFieldData? FieldsWhichIncludedInResponse { get; set; }
}
