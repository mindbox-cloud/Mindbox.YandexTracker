using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class GetIssuesRequest
{
	[DataMember(EmitDefaultValue = false, Name = "queue")]
	public string? Queue { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "keys")]
	public Collection<string>? Keys { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "filter")]
	public IssueFilter? Filter { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "query")]
	public string? Query { get; set; }

	[JsonIgnore]
	public IssuesExpandData? Expand { get; set; }
}

[DataContract]
public class IssueFilter
{
	[DataMember(EmitDefaultValue = false, Name = "lastCommentUpdatedAt")]
	public DateTime? LastCommentUpdatedAt { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "summary")]
	public string? Summary { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "parent")]
	public string? Parent { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "alliases")]
	public Collection<string>? Alliases { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "updatedBy")]
	public string? UpdatedBy { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "description")]
	public string? Description { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "sprint")]
	public Collection<string>? Sprints { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "type")]
	public IssueType? Type { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "priority")]
	public Priority? Priority { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "createdAt")]
	public DateTime? CreatedAt { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "followers")]
	public Collection<string>? Followers { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "createdBy")]
	public string? CreatedBy { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "votes")]
	public int? Votes { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "assignee")]
	public string? Assignee { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "project")]
	public string? Project { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "queue")]
	public string? Queue { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "updatedAt")]
	public DateTime? UpdatedAt { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "status")]
	public IssueStatus? Status { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "previousStatus")]
	public IssueStatus? PreviousStatus { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "favorite")]
	public bool? IsFavorite { get; set; }
}