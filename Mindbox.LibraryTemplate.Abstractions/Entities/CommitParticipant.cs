using System;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed class CommitParticipant
{
	[DataMember(Name = "date")]
	public DateTime Date { get; set; }
}