using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class GetAttachmentResponse
{
	[DataMember(Name = "id")]
	public required string Id { get; set; }

	[DataMember(Name = "name")]
	public required string Name { get; set; }

	[DataMember(Name = "content")]
	public required string Content { get; set; }

	[DataMember(Name = "thumbnail")]
	public required string Thumbnail { get; set; }

	[DataMember(Name = "createdBy")]
	public required FieldInfo CreatedBy { get; set; }

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; set; }

	[DataMember(Name = "mimetype")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public FileType Mimetype { get; set; }

	[DataMember(Name = "size")]
	public int Size { get; set; }

	[DataMember(Name = "metadata")]
	public AttachmentDataDto? Metadata { get; set; }
}

[DataContract]
public class AttachmentDataDto
{
	[DataMember(Name = "size")]
	public required int Size { get; set; }
}
