using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record Comment
{
	public int Id { get; init; }
	public required string Text { get; init; }
	public UserShortInfo CreatedBy { get; init; } = null!;
	public UserShortInfo? UpdatedBy { get; init; }
	public DateTime CreatedAt { get; init; }
	public DateTime? UpdatedAt { get; init; }
	public Collection<string> Attachments { get; init; } = [];
	public CommentType CommentType { get; init; }
	public CommentTransportType TransportType { get; init; }
	public Collection<string> Summonees { get; init; } = [];
	public Collection<string> MaillistSummonees { get; init; } = [];
}
