using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

internal sealed record CreateIssueResponse
{
	public required string Id { get; init; }

	public required string Key { get; init; }

	public required string Summary { get; init; }

	public DateTime? LastCommentUpdatedAt { get; init; }

	public FieldInfo? Parent { get; init; }

	public required FieldInfo UpdatedBy { get; init; }

	public FieldInfo? Author { get; init; }

	public string? Description { get; init; }

	[JsonPropertyName("sprint")]
	public Collection<FieldInfo> Sprints { get; init; } = [];

	public Collection<FieldInfo> Followers { get; init; } = [];

	public Collection<string> Aliases { get; init; } = [];

	public required FieldInfo Type { get; init; }

	public required FieldInfo Priority { get; init; }

	public DateTime CreatedAt { get; init; }

	public required FieldInfo CreatedBy { get; init; }

	public int Votes { get; init; }

	public FieldInfo? Assignee { get; init; }

	public FieldInfo? Project { get; init; }

	public required FieldInfo Queue { get; init; }

	public DateTime UpdatedAt { get; init; }

	public required FieldInfo Status { get; init; }

	public FieldInfo? PreviousStatus { get; init; }

	public bool IsFavorite { get; init; }

	[JsonExtensionData]
	public Dictionary<string, JsonElement> CustomFields { get; init; } = [];
}
