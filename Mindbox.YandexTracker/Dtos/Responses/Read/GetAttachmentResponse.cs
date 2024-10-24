using System;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record GetAttachmentResponse
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
	public string Mimetype { get; init; } = null!;

	[DataMember(Name = "size")]
	public int Size { get; init; }

	[DataMember(Name = "metadata")]
	public AttachmentMetadataDto? Metadata { get; init; }
}

[DataContract]
internal class AttachmentMetadataDto
{
	[DataMember(Name = "size")]
	public required string Size { get; init; }
}
