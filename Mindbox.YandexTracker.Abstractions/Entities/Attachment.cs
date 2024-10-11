using System;

namespace Mindbox.YandexTracker;

/// <summary>
/// Файл, прикрепленный к задаче
/// </summary>
public sealed record Attachment
{
	/// <summary>
	/// Идентификатор файла
	/// </summary>
	public required string Id { get; init; }

	/// <summary>
	/// Имя файла
	/// </summary>
	public required string Name { get; init; }

	/// <summary>
	/// Адрес для скачивания файла
	/// </summary>
	public required string Content { get; init; }

	/// <summary>
	/// Адрес для скачивания превью
	/// </summary>
	public required string Thumbnail { get; init; }

	/// <summary>
	/// Создатель файла
	/// </summary>
	public required UserShortInfo CreatedBy { get; init; }

	/// <summary>
	/// Дата создания файла
	/// </summary>
	public required DateTime CreatedAt { get; init; }

	/// <summary>
	/// Тип файла
	/// </summary>
	/// <value>text/plain
	/// <br/>image/png</value>
	public string MimeType { get; init; } = null!;

	/// <summary>
	/// Размер файла в байтах
	/// </summary>
	public int Size { get; init; }

	/// <summary>
	/// Объект с метаданными файла
	/// </summary>
	public AttachmentData? Metadata { get; init; }
}

public sealed record AttachmentData
{
	/// <summary>
	/// Геометрический размер изображения
	/// </summary>
	public required string Size { get; init; }
}