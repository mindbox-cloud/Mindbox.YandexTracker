using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

#pragma warning disable CA1711 // Идентификаторы не должны иметь неправильных суффиксов
public class Queue
#pragma warning restore CA1711 // Идентификаторы не должны иметь неправильных суффиксов
{
	public required int Id { get; set; }
	public required string Key { get; set; }
	public required string Name { get; set; }
	public required UserInfo Lead { get; set; }
	public string? Description { get; set; }
	public bool AssignAuto { get; set; }
	public required IssueType DefaultType { get; set; }
	public Priority DefaultPriority { get; set; }
	public Collection<UserInfo> TeamUsers { get; set; } = [];
	public Collection<IssueType> IssueTypes { get; set; } = [];
	public Collection<IssueTypeConfig> IssueTypesConfig { get; set; } = [];
	public Collection<IssueType> Workflows { get; set; } = [];
	public bool DenyVoting { get; set; }
}