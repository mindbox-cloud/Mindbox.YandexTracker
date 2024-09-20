using System.Collections.ObjectModel;
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class GetQueuesRequest
{
	[DataMember(EmitDefaultValue = false, Name = "input")]
	public string? Input { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "filter")]
	public QueueFieldsDto? Filter { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "orderBy")]
	public string? OrderBy { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "orderAsc")]
	public bool? OrderAscending { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "rootOnly")]
	public bool? RootOnly { get; set; }
}

[DataContract]
public class QueueFieldsDto
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

	[DataMember(EmitDefaultValue = false, Name = "start")]
	public DateTime? Start { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "end")]
	public DateTime? End { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "tags")]
	public Collection<string>? Tags { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "parentEntity")]
	public int? ParentEntityId { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "entityStatus")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public EntityStatus EntityStatus { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "quarter")]
	public object? Quarter { get; set; }
}