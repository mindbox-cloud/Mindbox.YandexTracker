using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record GetIssueResponse
{
	[DataMember(Name = "id")]
	public required string Id { get; init; }

	[DataMember(Name = "key")]
	public required string Key { get; init; }

	[DataMember(Name = "summary")]
	public required string Summary { get; init; }

	[DataMember(Name = "lastCommentUpdatedAt")]
	public DateTime? LastCommentUpdatedAt { get; init; }

	[DataMember(Name = "parent")]
	public FieldInfo? Parent { get; init; }

	[DataMember(Name = "aliases")]
	public Collection<string> Aliases { get; init; } = [];

	[DataMember(Name = "updatedBy")]
	public required FieldInfo UpdatedBy { get; init; }

	[DataMember(Name = "description")]
	public string? Description { get; init; }

	[DataMember(Name = "sprint")]
	public Collection<FieldInfo> Sprints { get; init; } = [];

	[DataMember(Name = "type")]
	public required FieldInfo Type { get; init; }

	[DataMember(Name = "priority")]
	public required FieldInfo Priority { get; init; }

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; init; }

	[DataMember(Name = "followers")]
	public Collection<FieldInfo> Followers { get; init; } = [];

	[DataMember(Name = "createdBy")]
	public required FieldInfo CreatedBy { get; init; }

	[DataMember(Name = "votes")]
	public int Votes { get; init; }

	[DataMember(Name = "assignee")]
	public FieldInfo? Assignee { get; init; }

	[DataMember(Name = "project")]
	public FieldInfo? Project { get; init; }

	[DataMember(Name = "queue")]
	public required FieldInfo Queue { get; init; }

	[DataMember(Name = "updatedAt")]
	public DateTime UpdatedAt { get; init; }

	[DataMember(Name = "status")]
	public required FieldInfo Status { get; init; }

	[DataMember(Name = "previousStatus")]
	public FieldInfo? PreviousStatus { get; init; }

	[DataMember(Name = "favorite")]
	public bool IsFavorite { get; init; }

	[JsonExtensionData]
	public Dictionary<string, object> Fields { get; init; } = [];
}
