using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record CreateIssueRequest
{
	[DataMember(Name = "summary")]
	public required string Summary { get; init; }

	[DataMember(Name = "queue")]
	public required string Queue { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "followers")]
	public Collection<string>? Followers { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "type")]
	[JsonConverter(typeof(StringEnumConverter))]
	public IssueType? Type { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "description")]
	public string? Description { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "parent")]
	public string? Parent { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "author")]
	public string? Author { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "unique")]
	public string? Unique { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "attachmentIds")]
	public Collection<string>? AttachmentsIds { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "sprint")]
	public Collection<string>? Sprints { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "priority")]
	[JsonConverter(typeof(StringEnumConverter))]
	public Priority? Priority { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "assignee")]
	public string? Assignee { get; init; }

	[DataMember(EmitDefaultValue = false)]
	[JsonExtensionData]
	public Dictionary<string, object>? Fields { get; init; }
}
