using System;

namespace Mindbox.YandexTracker;

/// <summary>
/// Файл/вложение, прикрепленный к задаче.
/// </summary>
public sealed record Attachment
{
	/// <summary>
	/// Идентификатор вложения.
	/// </summary>
	public required string Id { get; init; }

	/// <summary>
	/// Имя файла
	/// </summary>
	public required string Name { get; init; }

	/// <summary>
	/// Адрес для скачивания файла
	/// </summary>
	public required string ContentUrl { get; init; }

	/// <summary>
	/// Адрес для скачивания превью
	/// </summary>
	public string? ThumbnailUrl { get; init; }

	/// <summary>
	/// Создатель вложения.
	/// </summary>
	public required UserShortInfo CreatedBy { get; init; }

	/// <summary>
	/// Дата и время создания файла в UTC.
	/// </summary>
	public required DateTime CreatedAtUtc { get; init; }

	/// <summary>
	/// Тип файла
	/// </summary>
	/// <value>
	/// text/plain, image/png и т.д.
	/// </value>
	public string MimeType { get; init; } = null!;

	/// <summary>
	/// Размер файла в байтах
	/// </summary>
	public int SizeBytes { get; init; }

	/// <summary>
	/// Объект с метаданными файла
	/// </summary>
	public AttachmentMetadata? Metadata { get; init; }
}

/// <summary>
/// Объект с метаданными файла
/// </summary>
public sealed record AttachmentMetadata
{
	/// <summary>
	/// Геометрический размер изображения (ширина x высота в пикселях)
	/// </summary>
	public required string GeometricSize { get; init; }
}