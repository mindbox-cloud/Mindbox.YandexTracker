using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record GetProjectsResponse
{
	public int Hits { get; init; }

	public int Pages { get; init; }

	public required Collection<ProjectInfo> Values { get; init; }

	public string? OrderBy { get; init; }
}

public sealed record ProjectInfo
{
	public required string Id { get; init; }

	public int Version { get; init; }

	public int ShortId { get; init; }

	public ProjectEntityType EntityType { get; init; }

	public required UserShortInfoDto CreatedBy { get; init; }

	public DateTime CreatedAt { get; init; }

	public required DateTime UpdatedAt { get; init; }

	public ProjectInfoFields? Fields { get; init; }
}

public sealed record ProjectInfoFields
{
	public string? Summary { get; init; }

	public string? Description { get; init; }

	public UserShortInfoDto? Author { get; init; }

	public UserShortInfoDto? Lead { get; init; }

	public IReadOnlyCollection<UserShortInfoDto>? TeamUsers { get; init; }

	public IReadOnlyCollection<UserShortInfoDto>? Clients { get; init; }

	public IReadOnlyCollection<UserShortInfoDto>? Followers { get; init; }

	public IReadOnlyCollection<string>? Tags { get; init; }

	public DateOnly? Start { get; init; }

	public DateOnly? End { get; init; }

	public bool? TeamAccess { get; init; }

	public string? Status { get; init; }

	public IReadOnlyCollection<string>? Quarter { get; init; }

	public IReadOnlyCollection<CheckListItemDto>? ChecklistItems { get; init; }

	public IReadOnlyCollection<string>? IssueQueueKeys { get; init; }

	public FieldInfo? ParentEntity { get; init; }

	public ProjectEntityStatus? EntityStatus { get; init; }

	public IReadOnlyCollection<FieldInfo>? IssueQueues { get; init; }
}