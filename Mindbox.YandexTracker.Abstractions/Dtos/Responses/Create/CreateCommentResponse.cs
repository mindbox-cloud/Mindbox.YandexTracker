using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record CreateCommentResponse
{
	[DataMember(Name = "id")]
	public int Id { get; init; }

	[DataMember(Name = "text")]
	public required string Text { get; init; }

	[DataMember(Name = "createBody")]
	public required FieldInfo CreatedBy { get; init; }

	[DataMember(Name = "updateBody")]
	public required FieldInfo UpdatedBy { get; init; }

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; init; }

	[DataMember(Name = "updateAt")]
	public DateTime UpdatedAt { get; init; }

	[DataMember(Name = "summonees")]
	public Collection<FieldInfo> Summonees { get; init; } = [];

	[DataMember(Name = "maillistSummonees")]
	public Collection<FieldInfo> MaillistSummonees { get; init; } = [];

	[DataMember(Name = "type")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public CommentType Type { get; init; }

	[DataMember(Name = "transport")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public CommentTransportType TransportType { get; init; }
}
