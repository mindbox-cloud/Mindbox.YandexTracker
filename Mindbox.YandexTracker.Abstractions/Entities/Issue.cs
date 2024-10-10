using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record Issue
{
	public string Key { get; init; } = null!;
	public DateTime? LastCommentUpdatedAt { get; init; }
	public string Summary { get; init; } = default!;
	public string? Parent { get; init; }
	public UserShortInfo UpdatedBy { get; init; } = null!;
	public string? Description { get; init; }
	public IssueType Type { get; init; } = null!;
	public Priority Priority { get; init; }
	public DateTime CreatedAt { get; init; }
	public Collection<string> Aliases { get; init; } = [];
	public Collection<string> Sprints { get; init; } = [];
	public Collection<UserShortInfo> Followers { get; init; } = [];
	public UserShortInfo CreatedBy { get; init; } = null!;
	public int Votes { get; init; }
	public UserShortInfo? Assignee { get; init; }
	public UserShortInfo? Author { get; init; }
	public string? Project { get; init; }
	public required string Queue { get; init; } = null!;
	public DateTime UpdatedAt { get; init; }
	public IssueStatus Status { get; init; } = null!;
	public IssueStatus? PreviousStatus { get; init; }
	public bool IsFavorite { get; init; }
	public Dictionary<string, object> CustomFields { get; init; } = [];

	public override int GetHashCode()
	{
		var hashCodePart1 = HashCode.Combine(
			Key,
			LastCommentUpdatedAt,
			Summary,
			Parent,
			UpdatedBy,
			Description,
			Type,
			Priority
		);

		var hashCodePart2 = HashCode.Combine(
			CreatedAt,
			CreatedBy,
			Votes,
			Assignee,
			Author,
			Project,
			Queue,
			UpdatedAt
		);

		var hashCodePart3 = HashCode.Combine(
			Status,
			PreviousStatus,
			IsFavorite
		);

		var collectionHashCode = 0;

		foreach (var alias in Aliases)
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, alias.GetHashCode(StringComparison.InvariantCulture));
		}

		foreach (var sprint in Sprints)
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, sprint.GetHashCode(StringComparison.InvariantCulture));
		}

		foreach (var follower in Followers)
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, follower.GetHashCode());
		}

		foreach (var field in CustomFields)
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, field.Key, field.Value?.GetHashCode());
		}

		return HashCode.Combine(hashCodePart1, hashCodePart2, hashCodePart3, collectionHashCode);
	}
}