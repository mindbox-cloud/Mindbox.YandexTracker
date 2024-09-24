using System;
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

#pragma warning disable CA1819
#pragma warning disable IDE0301
	[DataMember(Name = "teamUsers")]
	public FieldInfo[] TeamUsers { get; set; } = Array.Empty<FieldInfo>();

	[DataMember(Name = "issueTypes")]
	public FieldInfo[] IssueTypes { get; set; } = Array.Empty<FieldInfo>();

	[DataMember(Name = "versions")]
	public FieldInfo[] Versions { get; set; } = Array.Empty<FieldInfo>();

	[DataMember(Name = "workflows")]
	public FieldInfo[] Workflows { get; set; } = Array.Empty<FieldInfo>();

	[DataMember(Name = "issueTypesConfig")]
	public IssueTypeConfigDto[] IssueTypesConfig { get; set; } = Array.Empty<IssueTypeConfigDto>();
#pragma warning restore IDE0301
#pragma warning restore CA1819

	[DataMember(Name = "denyVoting")]
	public bool DenyVoting { get; set; }
}
