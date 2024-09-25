using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class CreateIssueResponse
{
	[DataMember(Name = "id")]
	public required string Id { get; set; }

	[DataMember(Name = "key")]
	public required string Key { get; set; }

	[DataMember(Name = "summary")]
	public required string Summary { get; set; }

	[DataMember(Name = "lastCommentUpdatedAt")]
	public DateTime LastCommentUpdatedAt { get; set; }

	[DataMember(Name = "parent")]
	public FieldInfo? Parent { get; set; }

	[DataMember(Name = "updatedBy")]
	public required FieldInfo UpdatedBy { get; set; }

	[DataMember(Name = "description")]
	public string? Description { get; set; }

	[DataMember(Name = "sprint")]
	public Collection<string> Sprints { get; set; } = [];

	[DataMember(Name = "followers")]
	public Collection<FieldInfo> Followers { get; set; } = [];

	[DataMember(Name = "aliases")]
	public Collection<string> Aliases { get; set; } = [];

	[DataMember(Name = "type")]
	public required FieldInfo Type { get; set; }

	[DataMember(Name = "priority")]
	public required FieldInfo Priority { get; set; }

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; set; }

	[DataMember(Name = "createdBy")]
	public required FieldInfo CreatedBy { get; set; }

	[DataMember(Name = "votes")]
	public int Votes { get; set; }

	[DataMember(Name = "assignee")]
	public FieldInfo? Assignee { get; set; }

	[DataMember(Name = "project")]
	public FieldInfo? Project { get; set; }

	[DataMember(Name = "queue")]
	public required FieldInfo Queue { get; set; }

	[DataMember(Name = "updatedAt")]
	public DateTime UpdatedAt { get; set; }

	[DataMember(Name = "status")]
	public required FieldInfo Status { get; set; }

	[DataMember(Name = "previousStatus")]
	public FieldInfo? PreviousStatus { get; set; }

	[DataMember(Name = "favorite")]
	public bool IsFavorite { get; set; }
}
