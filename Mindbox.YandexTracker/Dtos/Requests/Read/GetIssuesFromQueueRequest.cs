using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record GetIssuesFromQueueRequest
{
	[DataMember(Name = "queue")]
	public required string QueueKey { get; init; }
}
