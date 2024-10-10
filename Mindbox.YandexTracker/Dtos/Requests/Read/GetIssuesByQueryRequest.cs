using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record GetIssuesByQueryRequest
{
	[DataMember(Name = "query")]
	public required string Query { get; init; }
}
