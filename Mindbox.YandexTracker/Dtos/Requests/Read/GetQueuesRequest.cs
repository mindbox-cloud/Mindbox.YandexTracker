using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record GetQueuesRequest
{
	[DataMember(EmitDefaultValue = false, Name = "input")]
	public string? Input { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "filter")]
	public QueueFieldsDto? Filter { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "orderBy")]
	public string? OrderBy { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "orderAsc")]
	public bool? OrderAscending { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "rootOnly")]
	public bool? RootOnly { get; init; }
}

[DataContract]
internal sealed record QueueFieldsDto
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

	[DataMember(EmitDefaultValue = false, Name = "start")]
	public DateTime? Start { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "end")]
	public DateTime? End { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "tags")]
	public Collection<string>? Tags { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "parentEntity")]
	public int? ParentEntityId { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "entityStatus")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public ProjectEntityStatus? EntityStatus { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "quarter")]
	public Collection<string>? Quarter { get; init; }
}