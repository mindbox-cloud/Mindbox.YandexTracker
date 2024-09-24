using System;

namespace Mindbox.YandexTracker;

#pragma warning disable CA1711
public class Queue
#pragma warning restore CA1711
{
	public required int Id { get; set; }
	public required string Key { get; set; }
	public required string Name { get; set; }
	public required UserInfo Lead { get; set; }
	public string? Description { get; set; }
	public bool AssignAuto { get; set; }
	public bool AllowExternals { get; set; }
	public required IssueType DefaultType { get; set; }
	public Priority DefaultPriority { get; set; }
#pragma warning disable CA1819
#pragma warning disable IDE0301
	public UserInfo[] TeamUsers { get; set; } = Array.Empty<UserInfo>();
	public IssueType[] IssueTypes { get; set; } = Array.Empty<IssueType>();
	public IssueTypeConfig[] IssueTypesConfig { get; set; } = Array.Empty<IssueTypeConfig>();
	public string[] Workflows { get; set; } = Array.Empty<string>();
#pragma warning restore IDE0301
#pragma warning restore CA1819
	public bool DenyVoting { get; set; }
}