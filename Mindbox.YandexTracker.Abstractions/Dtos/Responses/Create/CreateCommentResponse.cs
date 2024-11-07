using System;
using System.Collections.Generic;

namespace Mindbox.YandexTracker;

public sealed record CreateCommentResponse
{
	public int Id { get; init; }

	public int Version { get; init; }

	public required string LongId { get; init; }

	public string? Text { get; init; }

	public required UserShortInfoDto CreatedBy { get; init; }

	public required UserShortInfoDto UpdatedBy { get; init; }

	public DateTime CreatedAt { get; init; }

	public DateTime UpdatedAt { get; init; }

	public IReadOnlyCollection<UserShortInfoDto> Summonees { get; init; } = [];

	public IReadOnlyCollection<UserShortInfoDto> MaillistSummonees { get; init; } = [];

	public CommentType Type { get; init; }

	public CommentTransportType Transport { get; init; }
}
