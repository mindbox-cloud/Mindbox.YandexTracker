using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record CreateAttachmentResponse
{
	[DataMember(Name = "id")]
	public required string Id { get; init; }

	[DataMember(Name = "name")]
	public required string Name { get; init; }

	[DataMember(Name = "content")]
	public required string Content { get; init; }

	[DataMember(Name = "thumbnail")]
	public required string Thumbnail { get; init; }

	[DataMember(Name = "createdBy")]
	public required FieldInfo CreatedBy { get; init; }

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; init; }

	[DataMember(Name = "mimetype")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public FileType Mimetype { get; init; }

	[DataMember(Name = "size")]
	public int Size { get; init; }

	[DataMember(Name = "metadata")]
	public AttachmentDataDto? Metadata { get; init; }
}
