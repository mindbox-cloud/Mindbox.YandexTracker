using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

internal sealed record GetCommentsResponse
{
	public int Id { get; init; }

	public required string Text { get; init; }

	public Collection<FieldInfo> Attachments { get; init; } = [];

	public DateTime CreatedAt { get; init; }

	public DateTime UpdatedAt { get; init; }

	public required FieldInfo CreatedBy { get; init; }

	public required FieldInfo UpdatedBy { get; init; }

	public CommentType Type { get; init; }

	public CommentTransportType TransportType { get; init; }
}
