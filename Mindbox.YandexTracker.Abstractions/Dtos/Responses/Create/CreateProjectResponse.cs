using System;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

public sealed record CreateProjectResponse : CustomFieldsResponse
{
	public required string Id { get; init; }

	public int ShortId { get; init; }

	public ProjectEntityType EntityType { get; init; }

	public required UserShortInfoDto CreatedBy { get; init; }

	public DateTime CreatedAt { get; init; }

	public DateTime UpdatedAt { get; init; }
}
