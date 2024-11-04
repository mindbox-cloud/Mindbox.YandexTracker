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

public sealed record ProjectInfo : CustomFieldsResponse
{
	public required string Id { get; init; }

	public int ShortId { get; init; }

	public ProjectEntityType EntityType { get; init; }

	public required FieldInfo CreatedBy { get; init; }

	public DateTime CreatedAt { get; init; }

	public required DateTime UpdatedAt { get; init; }

	public string? Summary { get; init; }

	public string? Description { get; init; }

	public FieldInfo? Author { get; init; }

	public FieldInfo? Lead { get; init; }

	public IReadOnlyCollection<FieldInfo>? TeamUsers { get; init; }

	public IReadOnlyCollection<FieldInfo>? Clients { get; init; }

	public IReadOnlyCollection<FieldInfo>? Followers { get; init; }

	public IReadOnlyCollection<string>? Tags { get; init; }

	public DateTime? Start { get; init; }

	public DateTime? End { get; init; }

	public bool? TeamAccess { get; init; }

	public string? Status { get; init; }

	public IReadOnlyCollection<string>? Quarter { get; init; }

	public IReadOnlyCollection<FieldInfo>? ChecklistItems { get; init; }

	public IReadOnlyCollection<string>? IssueQueueKeys { get; init; }
}