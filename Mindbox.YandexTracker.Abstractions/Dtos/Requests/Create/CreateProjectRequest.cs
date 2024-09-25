using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class CreateProjectRequest
{
	[DataMember(Name = "fields")]
	public required ProjectFieldsDto Fields { get; set; }
}

[DataContract]
public class ProjectFieldsDto
{
	[DataMember(EmitDefaultValue = false, Name = "summary")]
	public required string Summary { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "teamAccess")]
	public bool? TeamAccess { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "description")]
	public string? Description { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "author")]
	public int? AuthorId { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "lead")]
	public int? LeadId { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "teamUsers")]
	public Collection<int>? TeamUsers { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "clients")]
	public Collection<int>? Clients { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "followers")]
	public Collection<int>? Followers { get; set; }

	/// <remarks>
	/// Must have YYYY-MM-DDThh:mm:ss.sss±hhmm format
	/// </remarks>
	[DataMember(EmitDefaultValue = false, Name = "start")]
	public DateTime? Start { get; set; }

	/// <remarks>
	/// Must have YYYY-MM-DDThh:mm:ss.sss±hhmm format
	/// </remarks>
	[DataMember(EmitDefaultValue = false, Name = "end")]
	public DateTime? End { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "tags")]
	public Collection<string>? Tags { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "parentEntity")]
	public int? ParentEntityId { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "entityStatus")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public ProjectEntityStatus EntityStatus { get; set; }
}