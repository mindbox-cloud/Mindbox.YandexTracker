using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record CreateQueueRequest
{
	[DataMember(Name = "key")]
	public required string Key { get; init; }

	[DataMember(Name = "name")]
	public required string Name { get; init; }

	[DataMember(Name = "lead")]
	public required string LeadKey { get; init; }

	[DataMember(Name = "defaultType")]
	public required string DefaultType { get; init; }

	[DataMember(Name = "defaultPriority")]
	public Priority DefaultPriority { get; init; }

	[DataMember(Name = "issueTypesConfig")]
	public Collection<CreateIssueTypeConfigDto> IssutTypesConfig { get; init; } = [];
}

[DataContract]
public sealed record CreateIssueTypeConfigDto
{
	[DataMember(Name = "issueType")]
	public required string IssueType { get; init; }

	[DataMember(Name = "workflow")]
	public required string Workflow { get; init; }

	[DataMember(Name = "resolutions")]
	public Collection<string> Resolutions { get; init; } = [];
}