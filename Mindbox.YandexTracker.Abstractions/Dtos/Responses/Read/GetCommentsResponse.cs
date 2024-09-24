using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class GetCommentsResponse
{
	[DataMember(Name = "id")]
	public int Id { get; set; }

	[DataMember(Name = "text")]
	public required string Text { get; set; }

#pragma warning disable CA1819
#pragma warning disable IDE0301
	[DataMember(Name = "attachments")]
	public FieldInfo[] Attachments { get; set; } = Array.Empty<FieldInfo>();
#pragma warning restore IDE0301
#pragma warning restore CA1819

	[DataMember(Name = "createdBy")]
	public required FieldInfo CreatedBy { get; set; }

	[DataMember(Name = "updatedBy")]
	public required FieldInfo UpdatedBy { get; set; }

	[DataMember(Name = "type")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public CommentType Type { get; set; }

	[DataMember(Name = "transport")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public CommentTransportType TransportType { get; set; }
}
