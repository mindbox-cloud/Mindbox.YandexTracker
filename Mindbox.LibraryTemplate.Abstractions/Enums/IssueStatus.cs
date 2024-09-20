using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public enum IssueStatus
{
	Open,
	InProcess,
	OnHold,
	Resolved,
	Canceled
}