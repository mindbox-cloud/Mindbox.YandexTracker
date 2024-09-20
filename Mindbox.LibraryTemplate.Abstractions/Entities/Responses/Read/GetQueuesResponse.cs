using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class GetQueuesResponse
{
	[DataMember(Name = "id")]
	public int Id { get; set; }

	[DataMember(Name = "key")]
	public required string Key { get; set; }

	[DataMember(Name = "description")]
	public required string Description { get; set; }

	[DataMember(Name = "lead")]
	public required FieldInfo Lead { get; set; }

	[DataMember(Name = "assignAuto")]
	public bool AssignAuto { get; set; }

	[DataMember(Name = "defaultType")]
	public required FieldInfo DefaultType { get; set; }

	[DataMember(Name = "defaultPriority")]
	public required FieldInfo DefaultPriority { get; set; }

	[DataMember(Name = "teamUsers")]
	public Collection<FieldInfo> TeamUsers { get; set; } = [];

	[DataMember(Name = "issueTypes")]
	public Collection<FieldInfo> IssueTypes { get; set; } = [];

	[DataMember(Name = "versions")]
	public Collection<FieldInfo> Versions { get; set; } = [];

	[DataMember(Name = "workflows")]
	public Collection<FieldInfo> Workflows { get; set; } = [];

	[DataMember(Name = "denyVoting")]
	public bool DenyVoting { get; set; }

	[DataMember(Name = "issueTypesConfig")]
	public Collection<IssueTypeConfigDto> IssueTypesConfig { get; set; } = [];
}
