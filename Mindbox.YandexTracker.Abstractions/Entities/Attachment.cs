using System;

namespace Mindbox.YandexTracker;

public sealed record Attachment
{
	public required string Id { get; init; }
	public required string Name { get; init; }
	public required string Content { get; init; }
	public required string Thumbnail { get; init; }
	public required UserShortInfo CreatedBy { get; init; }
	public required DateTime CreatedAt { get; init; }
	public string MimeType { get; init; } = null!;
	public int Size { get; init; }
	public AttachmentData? Metadata { get; init; }
}

public sealed record AttachmentData
{
	public required string Size { get; init; }
}