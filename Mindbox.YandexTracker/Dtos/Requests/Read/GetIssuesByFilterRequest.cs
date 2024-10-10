using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record GetIssuesByFilterRequest
{
	[DataMember(Name = "filter")]
	public required Dictionary<string, object> Filter { get; init; }
}
