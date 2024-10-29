using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

internal sealed record CreateProjectResponse
{
	public required string Id { get; init; }

	public int ShortId { get; init; }

	[JsonPropertyName("entityType")]
	public ProjectEntityType ProjectEntityType { get; init; }

	public required FieldInfo CreatedBy { get; init; }

	public DateTime CreatedAt { get; init; }

	public DateTime UpdatedAt { get; init; }

	[JsonExtensionData]
	public Dictionary<string, JsonElement> Fields { get; init; } = [];
}
