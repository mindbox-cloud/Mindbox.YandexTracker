using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

internal sealed record GetQueuesRequest
{
	public string? Input { get; init; }

	public QueueFieldsDto? Filter { get; init; }

	public string? OrderBy { get; init; }

	[JsonPropertyName("orderAsc")]
	public bool? OrderAscending { get; init; }

	public bool? RootOnly { get; init; }
}

internal sealed record QueueFieldsDto
{
	public required string Summary { get; init; }

	public bool? TeamAccess { get; init; }

	public string? Description { get; init; }

	[JsonPropertyName("author")]
	public int? AuthorId { get; init; }

	[JsonPropertyName("lead")]
	public int? LeadId { get; init; }

	public Collection<int>? TeamUsers { get; init; }

	public Collection<int>? Clients { get; init; }

	public Collection<int>? Followers { get; init; }

	public DateTime? Start { get; init; }

	public DateTime? End { get; init; }

	public Collection<string>? Tags { get; init; }

	[JsonPropertyName("parentEntity")]
	public int? ParentEntityId { get; init; }

	public ProjectEntityStatus? EntityStatus { get; init; }

	public Collection<string>? Quarter { get; init; }
}