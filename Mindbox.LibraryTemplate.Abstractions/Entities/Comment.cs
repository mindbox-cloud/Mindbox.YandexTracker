using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;
public class Comment
{
	public int Id { get; set; }
	public required string Text { get; set; }
	public required Account CreatedBy { get; set; }
	public required Account UpdatedBy { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public Collection<Account> Summonees { get; set; } = [];
	public Collection<Account> MaillistSummonees { get; set; } = [];
	public CommentType CommentType { get; set; }
	public CommentTransportType TransportType { get; set; }
}
