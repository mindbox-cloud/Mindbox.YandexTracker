using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class CreateCommentResponse
{
	[DataMember(Name = "id")]
	public int Id { get; set; }

	[DataMember(Name = "text")]
	public required string Text { get; set; }

	[DataMember(Name = "createBody")]
	public required FieldInfo CreatedBy { get; set; }

	[DataMember(Name = "updateBody")]
	public FieldInfo? UpdatedBy { get; set; }

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; set; }

	[DataMember(Name = "updateAt")]
	public DateTime UpdatedAt { get; set; }

	[DataMember(Name = "summonees")]
	public Collection<FieldInfo> Summonees { get; set; } = [];

	[DataMember(Name = "maillistSummonees")]
	public Collection<FieldInfo> MaillistSummonees { get; set; } = [];

	[DataMember(Name = "type")]
	public required string CommentType { get; set; }

	[DataMember(Name = "transport")]
	public required string TransportType { get; set; }
}
