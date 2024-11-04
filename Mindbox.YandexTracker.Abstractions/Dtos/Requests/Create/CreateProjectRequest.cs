using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

public sealed record CreateProjectRequest
{
	public required ProjectFieldsDto Fields { get; init; }
}

public sealed record ProjectFieldsDto
{
	public required string Summary { get; init; }

	public bool? TeamAccess { get; init; }

	public string? Description { get; init; }

	public string? Author { get; init; }

	public string? Lead { get; init; }

	public Collection<string>? TeamUsers { get; init; }

	public Collection<string>? Clients { get; init; }

	public Collection<string>? Followers { get; init; }

	/// <remarks>
	/// Must have YYYY-MM-DDThh:mm:ss.sss±hhmm format
	/// </remarks>
	public DateOnly? Start { get; init; }

	/// <remarks>
	/// WTF Дата начала в формате YYYY-MM-DDThh:mm:ss.sss±hhmm.
	/// </remarks>
	public DateOnly? End { get; init; }

	public Collection<string>? Tags { get; init; }

	[JsonPropertyName("parentEntity")]
	public int? ParentEntityId { get; init; }

	public ProjectEntityStatus? EntityStatus { get; init; }
}