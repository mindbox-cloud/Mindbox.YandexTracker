using System;

namespace Mindbox.YandexTracker;

public class Checklist
{
	public required int Id { get; set; }
	public required string Text { get; set; }
	public bool Checked { get; set; }
	public UserInfo? Assignee { get; set; }
	public DateTime? Deadline { get; set; }
}
