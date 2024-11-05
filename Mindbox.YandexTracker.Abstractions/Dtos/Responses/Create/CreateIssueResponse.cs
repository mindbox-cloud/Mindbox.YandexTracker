using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record CreateIssueResponse : CustomFieldsResponse
{
	public required string Id { get; init; }

	public required string Key { get; init; }

	public required int Version { get; init; }

	public required string Summary { get; init; }

	public DateTime? LastCommentUpdatedAt { get; init; }

	public FieldInfo? Parent { get; init; }

	public required UserShortInfoDto UpdatedBy { get; init; }

	public UserShortInfoDto? Author { get; init; }

	public UserShortInfoDto? QaEngineer { get; init; }

	public IReadOnlyCollection<UserShortInfoDto> Access { get; init; } = [];

	public int? PossibleSpam { get; init; }

	public string? Description { get; init; }

	public Collection<FieldInfo> Sprint { get; init; } = [];

	public Collection<UserShortInfoDto> Followers { get; init; } = [];

	public Collection<string> Aliases { get; init; } = [];

	public required FieldInfo Type { get; init; }

	public required FieldInfo Priority { get; init; }

	public DateTime CreatedAt { get; init; }

	public required UserShortInfoDto CreatedBy { get; init; }

	public int Votes { get; init; }

	public FieldInfo? Assignee { get; init; }

	public FieldInfo? Project { get; init; }

	public required FieldInfo Queue { get; init; }

	public DateTime UpdatedAt { get; init; }

	public required FieldInfo Status { get; init; }

	public FieldInfo? PreviousStatus { get; init; }

	public bool Favorite { get; init; }

	public DateOnly? Start { get; init; }

	public DateOnly? End { get; init; }

	public DateOnly? DueDate { get; init; }

	public DateTime? StatusStartTime { get; init; }
}
