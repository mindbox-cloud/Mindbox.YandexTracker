using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record Project
{
	public string Id { get; init; } = null!;
	public int ShortId { get; init; }
	public ProjectEntityType ProjectType { get; init; }
	public UserShortInfo CreatedBy { get; init; } = null!;
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
	public Collection<string>? Quarter { get; init; }
	public Collection<string>? ChecklistIds { get; init; }
	public int? ParentId { get; init; }
}
