using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record CreateComponentRequest
{
	[DataMember(Name = "name")]
	public required string Name { get; init; }

	[DataMember(Name = "queue")]
	public required string Queue { get; init; }
}
