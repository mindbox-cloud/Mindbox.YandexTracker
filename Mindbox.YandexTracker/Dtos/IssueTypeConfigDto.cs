using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record IssueTypeConfigDto
{
	[DataMember(Name = "issueType")]
	public required FieldInfo IssueType { get; init; }

	[DataMember(Name = "workflow")]
	public required FieldInfo Workflow { get; init; }

	[DataMember(Name = "resolutions")]
	public required Collection<FieldInfo> Resolutions { get; init; }
}
