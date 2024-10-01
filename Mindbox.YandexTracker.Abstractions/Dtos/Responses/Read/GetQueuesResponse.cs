using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record GetQueuesResponse
{
	[DataMember(Name = "id")]
	public int Id { get; init; }

	[DataMember(Name = "key")]
	public required string Key { get; init; }

	[DataMember(Name = "name")]
	public required string Name { get; init; }

	[DataMember(Name = "description")]
	public string? Description { get; init; }

	[DataMember(Name = "lead")]
	public required FieldInfo Lead { get; init; }

	[DataMember(Name = "assignAuto")]
	public bool AssignAuto { get; init; }

	[DataMember(Name = "defaultType")]
	public required FieldInfo DefaultType { get; init; }

	[DataMember(Name = "defaultPriority")]
	public required FieldInfo DefaultPriority { get; init; }

	[DataMember(Name = "teamUsers")]
	public Collection<FieldInfo> TeamUsers { get; init; } = [];

	[DataMember(Name = "issueTypes")]
	public Collection<FieldInfo> IssueTypes { get; init; } = [];

	[DataMember(Name = "versions")]
	public Collection<FieldInfo> Versions { get; init; } = [];

	[DataMember(Name = "workflows")]
	public WorkflowInfoDto? Workflows { get; init; }

	[DataMember(Name = "issueTypesConfig")]
	public Collection<IssueTypeConfigDto> IssueTypesConfigDto { get; init; } = [];

	[DataMember(Name = "denyVoting")]
	public bool DenyVoting { get; init; }
}

[DataContract]
public class WorkflowInfoDto
{
	[DataMember(Name = "dev")]
	public Collection<FieldInfo> Fields { get; init; } = [];
}