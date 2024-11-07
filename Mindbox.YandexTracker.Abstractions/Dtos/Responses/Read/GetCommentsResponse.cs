using System;
using System.Collections.Generic;

namespace Mindbox.YandexTracker;

public sealed record GetCommentsResponse
{
	public int Id { get; init; }

	public required string LongId { get; init; }

	public int Version { get; init; }

	public string? Text { get; init; }

	public string? TextHtml { get; init; }

	public IReadOnlyCollection<FieldInfo> Attachments { get; init; } = [];

	public DateTime CreatedAt { get; init; }

	public DateTime UpdatedAt { get; init; }

	public required UserShortInfoDto CreatedBy { get; init; }

	public required UserShortInfoDto UpdatedBy { get; init; }

	public CommentType Type { get; init; }

	public CommentTransportType Transport { get; init; }
}
