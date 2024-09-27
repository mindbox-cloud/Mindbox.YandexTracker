using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public class Comment
{
	public int Id { get; set; }
	public required string Text { get; set; }
	public required UserInfo CreatedBy { get; set; }
	public UserInfo? UpdatedBy { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public Collection<string> Attachments { get; set; } = [];
	public CommentType CommentType { get; set; }
	public CommentTransportType TransportType { get; set; }
}
