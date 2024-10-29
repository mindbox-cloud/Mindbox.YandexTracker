using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

internal sealed record CreateCommentResponse
{
	public int Id { get; init; }

	public required string Text { get; init; }

	public required FieldInfo CreatedBy { get; init; }

	public required FieldInfo UpdatedBy { get; init; }

	public DateTime CreatedAt { get; init; }

	public DateTime UpdatedAt { get; init; }

	public Collection<FieldInfo> Summonees { get; init; } = [];

	public Collection<FieldInfo> MaillistSummonees { get; init; } = [];

	public CommentType Type { get; init; }

	public CommentTransportType TransportType { get; init; }
}
