using System;
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
	public required FieldInfo UpdatedBy { get; set; }

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; set; }

	[DataMember(Name = "updateAt")]
	public DateTime UpdatedAt { get; set; }

#pragma warning disable CA1819
#pragma warning disable IDE0301
	[DataMember(Name = "summonees")]
	public FieldInfo[] Summonees { get; set; } = Array.Empty<FieldInfo>();

	[DataMember(Name = "maillistSummonees")]
	public FieldInfo[] MaillistSummonees { get; set; } = Array.Empty<FieldInfo>();
#pragma warning restore IDE0301
#pragma warning restore CA1819

	[DataMember(Name = "type")]
	public required string CommentType { get; set; }

	[DataMember(Name = "transport")]
	public required string TransportType { get; set; }
}
