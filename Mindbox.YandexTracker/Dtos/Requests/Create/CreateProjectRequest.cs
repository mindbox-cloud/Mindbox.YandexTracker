using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

internal sealed record CreateProjectRequest
{
	public required ProjectFieldsDto Fields { get; init; }
}

internal sealed record ProjectFieldsDto
{
	public required string Summary { get; init; }

	[JsonPropertyName("teamAccess")]
	public bool? TeamAccess { get; init; }

	public string? Description { get; init; }

	[JsonPropertyName("author")]
	public long? AuthorId { get; init; }

	[JsonPropertyName("lead")]
	public long? LeadId { get; init; }

	public Collection<long>? TeamUsers { get; init; }

	public Collection<long>? Clients { get; init; }

	public Collection<long>? Followers { get; init; }

	/// <remarks>
	/// Must have YYYY-MM-DDThh:mm:ss.sss±hhmm format
	/// </remarks>
	public DateTime? Start { get; init; }

	/// <remarks>
	/// Must have YYYY-MM-DDThh:mm:ss.sss±hhmm format
	/// </remarks>
	public DateTime? End { get; init; }

	public Collection<string>? Tags { get; init; }

	[JsonPropertyName("parentEntity")]
	public int? ParentEntityId { get; init; }

	public ProjectEntityStatus? EntityStatus { get; init; }
}