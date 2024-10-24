using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record GetCommentsResponse
{
	[DataMember(Name = "id")]
	public int Id { get; init; }

	[DataMember(Name = "text")]
	public required string Text { get; init; }

	[DataMember(Name = "attachments")]
	public Collection<FieldInfo> Attachments { get; init; } = [];

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; init; }

	[DataMember(Name = "updatedAt")]
	public DateTime UpdatedAt { get; init; }

	[DataMember(Name = "createdBy")]
	public required FieldInfo CreatedBy { get; init; }

	[DataMember(Name = "updatedBy")]
	public required FieldInfo UpdatedBy { get; init; }

	[DataMember(Name = "type")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public CommentType Type { get; init; }

	[DataMember(Name = "transport")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public CommentTransportType TransportType { get; init; }
}
