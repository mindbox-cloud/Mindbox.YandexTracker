using System;
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

#pragma warning disable CA1819
#pragma warning disable IDE0301
	[DataMember(Name = "sprint")]
	public string[] Sprints { get; set; } = Array.Empty<string>();

	[DataMember(Name = "followers")]
	public FieldInfo[] Followers { get; set; } = Array.Empty<FieldInfo>();

	[DataMember(Name = "aliases")]
	public string[] Aliases { get; set; } = Array.Empty<string>();
#pragma warning restore IDE0301
#pragma warning restore CA1819

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
