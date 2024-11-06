using System.Collections.Generic;

namespace Mindbox.YandexTracker;

public sealed record CreateCommentRequest
{
	public required string Text { get; init; }

	public IReadOnlyCollection<string> AttachmentIds { get; init; } = [];

	public IReadOnlyCollection<string> Summonees { get; init; } = [];

	public IReadOnlyCollection<string> MaillistSummonees { get; init; } = [];
}