using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

// TODO Прочекать доп поля.
public sealed record GetIssueResponse
{
	public required string Id { get; init; }

	public required string Key { get; init; }
	public required int Version { get; init; }

	public required string Summary { get; init; }

	public DateTime? LastCommentUpdatedAt { get; init; }

	public FieldInfo? Parent { get; init; }

	public Collection<string> Aliases { get; init; } = [];

	public required UserShortInfoDto UpdatedBy { get; init; }

	public string? Description { get; init; }

	public Collection<FieldInfo> Sprint { get; init; } = [];

	public required FieldInfo Type { get; init; }

	public FieldInfo? Author { get; init; }

	public required FieldInfo Priority { get; init; }

	public DateTime CreatedAt { get; init; }

	public Collection<UserShortInfoDto> Followers { get; init; } = [];

	public required UserShortInfoDto CreatedBy { get; init; }

	public int Votes { get; init; }

	public UserShortInfoDto? Assignee { get; init; }

	public FieldInfo? Project { get; init; }

	public required FieldInfo Queue { get; init; }

	public DateTime UpdatedAt { get; init; }

	public required FieldInfo Status { get; init; }

	public FieldInfo? PreviousStatus { get; init; }

	public bool Favorite { get; init; }

	public DateOnly? Start { get; set; }

	public IReadOnlyCollection<string> Tags { get; init; } = [];

	[JsonExtensionData]
	public Dictionary<string, JsonElement> Fields { get; init; } = [];
}
