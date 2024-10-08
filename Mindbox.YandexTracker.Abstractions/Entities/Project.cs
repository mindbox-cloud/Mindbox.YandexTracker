using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record Project
{
	public required string Id { get; init; }
	public required int ShortId { get; init; }
	public ProjectEntityType ProjectType { get; init; }
	public required UserShortInfo CreatedBy { get; init; }
	public DateTime CreatedAt { get; init; }
	public DateTime UpdatedAt { get; init; }
	public string? Summary { get; init; }
	public string? Description { get; init; }
	public UserShortInfo? Author { get; init; }
	public UserShortInfo? Lead { get; init; }
	public Collection<UserShortInfo>? TeamUsers { get; init; }
	public Collection<UserShortInfo>? Clients { get; init; }
	public Collection<UserShortInfo>? Followers { get; init; }
	public Collection<string>? Tags { get; init; } 
	public DateTime? Start { get; init; }
	public DateTime? End { get; init; }
	public bool? TeamAccess { get; init; }
	public ProjectEntityStatus? Status { get; init; }
	public Collection<FieldInfo>? IssueQueues { get; init; }
	public Collection<string>? Quarter { get; init; }
	public Collection<string>? ChecklistIds { get; init; }
}
