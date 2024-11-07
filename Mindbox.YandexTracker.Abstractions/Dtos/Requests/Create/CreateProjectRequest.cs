using System;
using System.Collections.Generic;

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

	public IReadOnlyCollection<string>? TeamUsers { get; init; }

	public IReadOnlyCollection<string>? Clients { get; init; }

	public IReadOnlyCollection<string>? Followers { get; init; }

	public DateOnly? Start { get; init; }

	public DateOnly? End { get; init; }

	public IReadOnlyCollection<string>? Tags { get; init; }

	public int? ParentEntity { get; init; }

	public ProjectEntityStatus? EntityStatus { get; init; }
}