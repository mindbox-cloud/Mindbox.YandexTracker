using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record Issue
{
	public required string Key { get; init; }
	public DateTime? LastCommentUpdatedAt { get; init; }
	public string Summary { get; init; } = default!;
	public string? Parent { get; init; }
	public required UserShortInfo UpdatedBy { get; init; }
	public string? Description { get; init; }
	public required IssueType Type { get; init; }
	public Priority Priority { get; init; }
	public DateTime CreatedAt { get; init; }
	public Collection<string> Aliases { get; init; } = [];
	public Collection<string> Sprints { get; init; } = [];
	public Collection<UserShortInfo> Followers { get; init; } = [];
	public required UserShortInfo CreatedBy { get; init; }
	public int Votes { get; init; }
	public UserShortInfo? Assignee { get; init; }
	public string? Project { get; init; }
	public required string Queue { get; init; }
	public DateTime UpdatedAt { get; init; }
	public required IssueStatus Status { get; init; }
	public IssueStatus? PreviousStatus { get; init; }
	public bool IsFavorite { get; init; }
	public Dictionary<string, object> CustomFields { get; init; } = [];
}