using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed class Commit
{
	[DataMember(Name = "sha")]
	public required string Sha { get; set; }

	[DataMember(Name = "details")]
	public required CommitDetails CommitDetails { get; set; }
}