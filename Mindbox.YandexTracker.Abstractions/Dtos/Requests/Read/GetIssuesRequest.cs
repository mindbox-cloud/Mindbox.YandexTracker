using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record GetIssuesRequest
{
	[DataMember(EmitDefaultValue = false, Name = "queue")]
	public string? Queue { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "keys")]
	public Collection<string>? Keys { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "filter")]
	public IssueFilter? Filter { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "query")]
	public string? Query { get; init; }

	[JsonIgnore]
	public IssuesExpandData? Expand { get; init; }
}

[DataContract]
public sealed record IssueFilter
{
	[DataMember(EmitDefaultValue = false, Name = "lastCommentUpdatedAt")]
	public DateTime? LastCommentUpdatedAt { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "summary")]
	public string? Summary { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "parent")]
	public string? Parent { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "alliases")]
	public Collection<string>? Alliases { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "updatedBy")]
	public string? UpdatedBy { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "description")]
	public string? Description { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "sprint")]
	public Collection<string>? Sprints { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "type")]
	public IssueType? Type { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "priority")]
	public Priority? Priority { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "createdAt")]
	public DateTime? CreatedAt { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "followers")]
	public Collection<string>? Followers { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "createdBy")]
	public string? CreatedBy { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "votes")]
	public int? Votes { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "assignee")]
	public string? Assignee { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "project")]
	public string? Project { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "queue")]
	public string? Queue { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "updatedAt")]
	public DateTime? UpdatedAt { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "status")]
	public IssueStatus? Status { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "previousStatus")]
	public IssueStatus? PreviousStatus { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "favorite")]
	public bool? IsFavorite { get; init; }
}