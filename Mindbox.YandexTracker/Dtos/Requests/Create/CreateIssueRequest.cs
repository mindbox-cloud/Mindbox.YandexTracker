using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

internal sealed record CreateIssueRequest
{
	public required string Summary { get; init; }

	public required string Queue { get; init; }

	public Collection<string>? Followers { get; init; }

	public IssueType? Type { get; init; }

	public string? Description { get; init; }

	public string? Parent { get; init; }

	public DateOnly? Start { get; init; }

	public string? Author { get; init; }

	public string? Unique { get; init; }

	public Collection<string>? AttachmentsIds { get; init; }

	[JsonPropertyName("sprint")]
	public Collection<string>? Sprints { get; init; }

	public Priority? Priority { get; init; }

	public string? Assignee { get; init; }

	public IReadOnlyCollection<string> Tags { get; init; } = [];

	[JsonExtensionData]
	public IDictionary<string, JsonElement> Fields { get; init; } = new Dictionary<string, JsonElement>();
}
