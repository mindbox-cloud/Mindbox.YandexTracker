using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record GetIssuesFromKeysRequest
{
	[DataMember(Name = "keys")]
	public Collection<string> Keys { get; init; } = [];
}
