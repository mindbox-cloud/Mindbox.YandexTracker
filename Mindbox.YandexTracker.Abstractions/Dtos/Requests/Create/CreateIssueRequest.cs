using System.Collections.Generic;
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

	[DataMember(EmitDefaultValue = false, Name = "followers")]
	public Collection<FieldInfo>? Followers { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "type")]
	public IssueType? Type { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "description")]
	public string? Description { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "parent")]
	public string? Parent { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "author")]
	public string? Author { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "unique")]
	public string? Unique { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "attachmentIds")]
	public Collection<string>? AttachemntsIds { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "sprint")]
	public Collection<string>? Sprints { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "priority")]
	public Priority? Priority { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "assignee")]
	public string? Assignee { get; set; }

	[DataMember(EmitDefaultValue = false)]
	public Dictionary<string, List<object>>? Fields { get; set; } 
}
