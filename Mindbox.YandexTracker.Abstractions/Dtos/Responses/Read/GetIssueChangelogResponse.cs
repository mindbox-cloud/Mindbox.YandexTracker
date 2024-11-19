using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Mindbox.YandexTracker;

public sealed record GetIssueChangelogResponse
{
	public required string Id { get; init; }
	public required FieldInfo Issue { get; init; }
	public required DateTime UpdatedAt { get; init; }
	public UserShortInfoDto? UpdatedBy { get; init; }
	public string? Transport { get; init; }
	public IssueChangeType Type { get; init; }
	public IReadOnlyCollection<FieldChangeDto> Fields { get; init; } = [];
	public CommentsDto? Comments { get; init; }
	public IReadOnlyCollection<ExecutedTriggersDto> ExecutedTriggers { get; init; } = [];
}

public sealed record CommentsDto
{
	public IReadOnlyCollection<FieldInfo> Added { get; init; } = [];
}

public sealed record FieldChangeDto
{
	public required FieldInfo Field { get; init; }
	public JsonElement? From { get; init; }
	public JsonElement? To { get; init; }
}

public sealed record ExecutedTriggersDto
{
	public bool Success { get; init; }
	public string? Message { get; init; }
	public required FieldInfo Trigger { get; init; }
}