using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record CreateProjectRequest
{
	[DataMember(Name = "fields")]
	public required ProjectFieldsDto Fields { get; init; }

	[JsonIgnore]
	public ProjectFieldData? ReturnedFields { get; init; }
}

[DataContract]
public sealed record ProjectFieldsDto
{
	[DataMember(EmitDefaultValue = false, Name = "summary")]
	public required string Summary { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "teamAccess")]
	public bool? TeamAccess { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "description")]
	public string? Description { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "author")]
	public int? AuthorId { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "lead")]
	public int? LeadId { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "teamUsers")]
	public Collection<int>? TeamUsers { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "clients")]
	public Collection<int>? Clients { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "followers")]
	public Collection<int>? Followers { get; init; }

	/// <remarks>
	/// Must have YYYY-MM-DDThh:mm:ss.sss±hhmm format
	/// </remarks>
	[DataMember(EmitDefaultValue = false, Name = "start")]
	public DateTime? Start { get; init; }

	/// <remarks>
	/// Must have YYYY-MM-DDThh:mm:ss.sss±hhmm format
	/// </remarks>
	[DataMember(EmitDefaultValue = false, Name = "end")]
	public DateTime? End { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "tags")]
	public Collection<string>? Tags { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "parentEntity")]
	public int? ParentEntityId { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "entityStatus")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public ProjectEntityStatus EntityStatus { get; init; }
}