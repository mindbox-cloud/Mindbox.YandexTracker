using System;
using System.Collections.Generic;

namespace Mindbox.YandexTracker;

public sealed record CreateIssueRequest : CustomFieldsRequest
{
	public required string Summary { get; init; }

	public required string Queue { get; init; }

	public IReadOnlyCollection<string>? Followers { get; init; }

	public string? Type { get; init; }

	public string? Description { get; init; }

	public string? Parent { get; init; }

	public DateOnly? Start { get; init; }
	public DateOnly? End { get; init; }
	public DateOnly? DueDate { get; init; }

	public string? Author { get; init; }

	public string? QaEngineer { get; init; }

	public IReadOnlyCollection<string>? Access { get; init; }

	public int? PossibleSpam { get; init; }

	public string? Unique { get; init; }

	public IReadOnlyCollection<string>? AttachmentIds { get; init; }

	public IReadOnlyCollection<string>? Sprint { get; init; }

	public Priority? Priority { get; init; }

	public string? Assignee { get; init; }

	public IReadOnlyCollection<string>? Tags { get; init; }
}
