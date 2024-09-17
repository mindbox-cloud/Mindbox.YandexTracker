using System;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed class CommitStatus
{
	[DataMember(Name = "state")]
	public CommitStatusState State { get; set; }

	[DataMember(Name = "created_at")]
	public DateTime CreatedAt { get; set; }

	[DataMember(Name = "target_url")]
	public required string TargetUrl { get; set; }

	[DataMember(Name = "context")]
	public required string Context { get; set; }
}