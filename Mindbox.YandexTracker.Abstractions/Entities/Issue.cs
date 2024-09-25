using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed class Issue
{
	public DateTime? LastCommentUpdatedAt { get; set; }
	public string Summary { get; set; } = default!;
	public string? Parent { get; set; }
	public required UserInfo UpdatedBy { get; set; }
	public string? Description { get; set; }
	public required IssueType Type { get; set; }
	public Priority Priority { get; set; }
	public DateTime CreatedAt { get; set; }
	public Collection<string> Alliases { get; set; } = [];
	public Collection<string> Sprints { get; set; } = [];
	public Collection<UserInfo> Followers { get; set; } = [];
	public required UserInfo CreatedBy { get; set; }
	public int Votes { get; set; }
	public UserInfo? Assignee { get; set; }
	public string? Project { get; set; }
	public required string Queue { get; set; }
	public DateTime UpdatedAt { get; set; }
	public required IssueStatus Status { get; set; }
	public IssueStatus? PreviousStatus { get; set; }
	public bool IsFavorite { get; set; }
}