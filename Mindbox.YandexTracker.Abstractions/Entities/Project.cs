using System;

namespace Mindbox.YandexTracker;

public class Project
{
	public required string Summary { get; set; }
	public string? Description { get; set; }
	public required UserInfo Author { get; set; }
	public required UserInfo Lead { get; set; }
#pragma warning disable CA1819
#pragma warning disable IDE0301
	public UserInfo[] TeamUsers { get; set; } = Array.Empty<UserInfo>();
	public UserInfo[] Clients { get; set; } = Array.Empty<UserInfo>();
	public UserInfo[] Followers { get; set; } = Array.Empty<UserInfo>();
	public string[] Tags { get; set; } = Array.Empty<string>();
#pragma warning restore IDE0301
#pragma warning restore CA1819
	public DateTime? Start { get; set; }
	public DateTime? End { get; set; }
	public bool TeamAccess { get; set; }
	public ProjectEntityStatus Status { get; set; }
}
