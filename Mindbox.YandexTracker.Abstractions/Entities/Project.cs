using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record Project
{
	public required string Id { get; set; }
	public ProjectEntityType ProjectType { get; set; }
	public required UserInfo CreatedBy { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
	public string? Summary { get; set; }
	public string? Description { get; set; }
	public UserInfo? Author { get; set; }
	public UserInfo? Lead { get; set; }
	public Collection<UserInfo>? TeamUsers { get; set; }
	public Collection<UserInfo>? Clients { get; set; }
	public Collection<UserInfo>? Followers { get; set; }
	public Collection<string>? Tags { get; set; } 
	public DateTime? Start { get; set; }
	public DateTime? End { get; set; }
	public bool? TeamAccess { get; set; }
	public ProjectEntityStatus? Status { get; set; }
	public Collection<FieldInfo>? IssueQueues { get; set; }
	public Collection<string>? Quarter { get; set; }
	public Collection<string>? ChecklistIds { get; set; }
}
