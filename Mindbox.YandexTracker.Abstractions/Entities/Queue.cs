using System.Collections.ObjectModel;

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
	public Collection<UserInfo> eamUsers { get; set; } = [];
	public Collection<IssueType> ssueTypes { get; set; } = [];
	public Collection<IssueTypeConfig> ssueTypesConfig { get; set; } = [];
	public Collection<string> Workflows { get; set; } = [];
	public bool DenyVoting { get; set; }
}