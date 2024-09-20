using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;
public class QueueEntity
{
	public required int Id { get; set; }
	public required string Key { get; set; }
	public required string Name { get; set; }
	public required Account Lead { get; set; }
	public string? Description { get; set; }
	public bool AssignAuto { get; set; }
	public bool AllowExternals { get; set; }
	public IssueType DefaultType { get; set; }
	public Priority DefaultPriority { get; set; }
	public Collection<Account> TeamUsers { get; set; } = [];
	public Collection<IssueType> IssueTypes { get; set; } = [];
	public bool DenyVoting { get; set; }
	public Collection<IssueTypeConfig> IssueTypesConfig { get; set; } = [];
	public Collection<string> Workflows { get; set; } = [];
}