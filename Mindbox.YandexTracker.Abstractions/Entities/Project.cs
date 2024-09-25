using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public class Project
{
	public required string Summary { get; set; }
	public string? Description { get; set; }
	public required UserInfo Author { get; set; }
	public required UserInfo Lead { get; set; }
	public Collection<UserInfo> eamUsers { get; set; } = [];
	public Collection<UserInfo> lients { get; set; } = [];
	public Collection<UserInfo> ollowers { get; set; } = [];
	public Collection<string> Tags { get; set; } = [];
	public DateTime? Start { get; set; }
	public DateTime? End { get; set; }
	public bool TeamAccess { get; set; }
	public ProjectEntityStatus Status { get; set; }
}
