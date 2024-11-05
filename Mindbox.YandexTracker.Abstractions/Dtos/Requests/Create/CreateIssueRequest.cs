using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

// TODO Чекнуть доп поля
public sealed record CreateIssueRequest : CustomFieldsRequest
{
	public required string Summary { get; init; }

	public required string Queue { get; init; }

	public Collection<string>? Followers { get; init; }

	public string? Type { get; init; }

	public string? Description { get; init; }

	public string? Parent { get; init; }

	public DateOnly? Start { get; init; }

	public string? Author { get; init; }

	public string? Unique { get; init; }

	public Collection<string>? AttachmentsIds { get; init; }

	public Collection<string>? Sprint { get; init; }

	public Priority? Priority { get; init; }

	public string? Assignee { get; init; }

	public IReadOnlyCollection<string>? AttachmentIds { get; init; }

	public IReadOnlyCollection<string> Tags { get; init; } = [];
}
