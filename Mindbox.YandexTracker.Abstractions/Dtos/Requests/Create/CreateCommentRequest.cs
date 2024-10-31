using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record CreateCommentRequest
{
	public required string Text { get; init; }

	public Collection<string>? AttachmentIds { get; init; }

	public Collection<string>? Summonees { get; init; }

	public Collection<string>? MaillistSummonees { get; init; }
}