using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed class CommitStatusesDto
{
	[DataMember(Name = "statuses")]
	public required List<CommitStatus> Statuses { get; set; }
}