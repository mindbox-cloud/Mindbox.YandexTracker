using System;

namespace Mindbox.YandexTracker;

public sealed record CreateAttachmentResponse
{
	public required string Id { get; init; }

	public required string Name { get; init; }

	public required string Content { get; init; }

	public string? Thumbnail { get; init; }

	public required UserShortInfoDto CreatedBy { get; init; }

	public DateTime CreatedAt { get; init; }

	public string Mimetype { get; init; } = null!;

	public int Size { get; init; }

	public AttachmentMetadataDto? Metadata { get; init; }
}
