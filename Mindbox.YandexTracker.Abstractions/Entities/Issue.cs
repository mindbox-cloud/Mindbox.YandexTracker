using System;

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
#pragma warning disable CA1819
#pragma warning disable IDE0301
	public string[] Alliases { get; set; } = Array.Empty<string>();
	public string[] Sprints { get; set; } = Array.Empty<string>();
	public UserInfo[] Followers { get; set; } = Array.Empty<UserInfo>();
#pragma warning restore IDE0301
#pragma warning restore CA1819
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