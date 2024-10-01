using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record CreateCommentResponse
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

	[DataMember(Name = "summonees")]
	public Collection<FieldInfo> Summonees { get; set; } = [];

	[DataMember(Name = "maillistSummonees")]
	public Collection<FieldInfo> MaillistSummonees { get; set; } = [];

	[DataMember(Name = "type")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public CommentType Type { get; set; }

	[DataMember(Name = "transport")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public CommentTransportType TransportType { get; set; }
}
