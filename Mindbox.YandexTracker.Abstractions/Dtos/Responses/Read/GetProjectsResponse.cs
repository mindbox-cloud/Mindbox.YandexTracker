using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

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

	public int ShortId { get; init; }

	[JsonPropertyName("entityType")]
	public ProjectEntityType ProjectType { get; init; }

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

	[JsonPropertyName("teamAccess")]
	public bool? TeamAccess { get; init; }

	public string? Status { get; init; }

	public IReadOnlyCollection<string>? Quarter { get; init; }

	[JsonPropertyName("checklistItems")]
	public IReadOnlyCollection<FieldInfo>? ChecklistIds { get; init; }

	public IReadOnlyCollection<string>? IssueQueueKeys { get; init; }

	[JsonExtensionData]
	public IReadOnlyDictionary<string, JsonElement> Fields { get; init; } = new Dictionary<string, JsonElement>();
}