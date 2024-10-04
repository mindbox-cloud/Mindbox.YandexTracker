using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

// Queue - зарезервированное слово, идентификатор не должен на него оканчиваться, CA1711 на это ругается
// Но название норм по бизнесу, поэтому повесим прагму.
#pragma warning disable CA1711
public sealed record Queue
#pragma warning restore CA1711
{
	public required string Id { get; init; }
	public required string Key { get; init; }
	public required string Name { get; init; }
	public required UserInfo Lead { get; init; }
	public string? Description { get; init; }
	public bool AssignAuto { get; init; }
	public required IssueType DefaultType { get; init; }
	public Priority DefaultPriority { get; init; }
	public Collection<UserInfo> TeamUsers { get; init; } = [];
	public Collection<IssueType> IssueTypes { get; init; } = [];
	public Collection<IssueTypeConfig> IssueTypesConfig { get; init; } = [];
	public Collection<IssueType> Workflows { get; init; } = [];
	public bool DenyVoting { get; init; }
}