using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

// Queue - зарезервированное слово, идентификатор не должен на него оканчиваться, CA1711 на это ругается
// Но название норм по бизнесу, поэтому повесим прагму.
#pragma warning disable CA1711
public sealed record Queue
#pragma warning restore CA1711
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