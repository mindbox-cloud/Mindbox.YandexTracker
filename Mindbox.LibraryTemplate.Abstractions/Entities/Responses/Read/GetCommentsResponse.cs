using System.Collections.ObjectModel;
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

	[DataMember(Name = "attachments")]
	public Collection<FieldInfo> Attachment { get; set; } = [];

	[DataMember(Name = "createdBy")]
	public required FieldInfo CreatedBy { get; set; }

	[DataMember(Name = "updatedBy")]
	public FieldInfo? UpdatedBy { get; set; }

	[DataMember(Name = "type")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public CommentType Type { get; set; }

	[DataMember(Name = "transport")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public CommentTransportType TransportType { get; set; }
}
