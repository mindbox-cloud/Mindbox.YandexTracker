using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

	public int? ParentEntity { get; init; }

	public ProjectEntityStatus? EntityStatus { get; init; }

	public IReadOnlyCollection<string> Quarter { get; init; } = [];
}