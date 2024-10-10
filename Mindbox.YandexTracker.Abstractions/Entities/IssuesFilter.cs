using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record IssuesFilter
{
	[DataMember(EmitDefaultValue = false, Name = "lastCommentUpdatedAt")]
	public DateTime? LastCommentUpdatedAt { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "summary")]
	public string? Summary { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "parent")]
	public string? Parent { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "alliases")]
	public Collection<string>? Aliases { get; init; }

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

	public override int GetHashCode()
	{
		var hashCodePart1 = HashCode.Combine(
			LastCommentUpdatedAt,
			Summary,
			Parent,
			UpdatedBy,
			Description,
			Type,
			Priority,
			CreatedAt);

		var hashCodePart2 = HashCode.Combine(
			CreatedBy,
			Votes,
			Assignee,
			Project,
			UpdatedAt,
			Status,
			PreviousStatus,
			IsFavorite);

		var collectionHashCode = 0;

		foreach (var alias in Aliases ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, alias.GetHashCode(StringComparison.InvariantCulture));
		}

		foreach (var sprint in Sprints ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, sprint.GetHashCode(StringComparison.InvariantCulture));
		}

		foreach (var follower in Followers ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, follower.GetHashCode(StringComparison.InvariantCulture));
		}

		return HashCode.Combine(hashCodePart1, hashCodePart2, collectionHashCode, Queue);
	}
}
