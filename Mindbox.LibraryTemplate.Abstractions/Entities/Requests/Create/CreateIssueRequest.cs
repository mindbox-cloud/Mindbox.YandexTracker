using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class CreateIssueRequest
{
	[DataMember(Name = "summary")]
	public required string Summary { get; set; }

	[DataMember(Name = "queue")]
	public required FieldInfo Queue { get; set; }

	[DataMember(Name = "followers")]
	public Collection<FieldInfo>? Followers { get; set; }

	[DataMember(Name = "type")]
	public IssueType? Type { get; set; }

	[DataMember(Name = "description")]
	public string? Description { get; set; }

	[DataMember(Name = "parent")]
	public string? Parent { get; set; }

	[DataMember(Name = "author")]
	public string? Author { get; set; }

	[DataMember(Name = "unique")]
	public string? Unique { get; set; }

	[DataMember(Name = "attachmentIds")]
	public Collection<string>? AttachemntsIds { get; set; }

	[DataMember(Name = "sprint")]
	public Collection<string>? Sprints { get; set; }

	[DataMember(Name = "priority")]
	public Priority? Priority { get; set; }

	[DataMember(Name = "assignee")]
	public string? Assignee { get; set; }
}
