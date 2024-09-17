using System;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed class PullRequest
{
	[DataMember(Name = "number")]
	public int Number { get; set; }

	[DataMember(Name = "created_at")]
	public DateTime CreatedAt { get; set; }

	[DataMember(Name = "closed_at")]
	public DateTime? ClosedAt { get; set; }

	[DataMember(Name = "head")]
	public required Commit Head { get; set; }

	[DataMember(Name = "title")]
	public required string Title { get; set; }
}