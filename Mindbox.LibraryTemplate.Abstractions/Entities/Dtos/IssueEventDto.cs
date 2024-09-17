using System;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed class IssueEventDto
{
	[DataMember(Name = "event")]
	public required string Event { get; set; }

	[DataMember(Name = "created_at")]
	public DateTime CreatedAt { get; set; }

	[DataMember(Name = "issue")]
	public required IssueDto Issue { get; set; }
}