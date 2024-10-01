using System;

namespace Mindbox.YandexTracker;

public sealed record Attachment
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public required string Content { get; set; }
	public required string Thumbnail { get; set; }
	public required UserInfo CreatedBy { get; set; }
	public required DateTime CreatedAt { get; set; }
	public FileType MimeType { get; set; }
	public int Size { get; set; }
	public AttachmentData? Metadata { get; set; }
}

public sealed record AttachmentData
{
	public required string Size { get; set; }
}