using System;

namespace Mindbox.YandexTracker;

public class Comment
{
	public int Id { get; set; }
	public required string Text { get; set; }
	public required UserInfo CreatedBy { get; set; }
	public UserInfo? UpdatedBy { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
#pragma warning disable CA1819
#pragma warning disable IDE0301
	public Attachment[] Attachments { get; set; } = Array.Empty<Attachment>();
#pragma warning restore IDE0301
#pragma warning restore CA1819
	public CommentType CommentType { get; set; }
	public CommentTransportType TransportType { get; set; }
}
