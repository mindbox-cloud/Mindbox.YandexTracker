using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record IssueTypeConfigDto
{
	[DataMember(Name = "issueType")]
	public required FieldInfo IssueType { get; set; }

	[DataMember(Name = "workflow")]
	public required FieldInfo Workflow { get; set; }

	[DataMember(Name = "resolutions")]
	public required Collection<FieldInfo> Resolutions { get; set; }
}
