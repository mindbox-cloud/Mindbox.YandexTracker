using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

/// <summary>
/// Комментарий к задаче
/// </summary>
public sealed record Comment
{

	/// <summary>
	/// Идентификатор комментария
	/// </summary>
	public int Id { get; init; }

	/// <summary>
	/// Текст комментария
	/// </summary>
	public required string Text { get; init; }

	/// <summary>
	/// Автор комментария.
	/// </summary>
	public UserShortInfo CreatedBy { get; init; } = null!;

	/// <summary>
	/// Пользователь, который внес последнее изменение в комментарий.
	/// </summary>
	public UserShortInfo? UpdatedBy { get; init; }

	/// <summary>
	/// Дата и время создания комментария в UTC.
	/// </summary>
	public DateTime CreatedAtUtc { get; init; }

	/// <summary>
	/// Дата и время обновления комментария в UTC.
	/// </summary>
	public DateTime? UpdatedAtUtc { get; init; }

	/// <summary>
	/// Вложения, связанные с комментарием.
	/// </summary>
	public Collection<string> Attachments { get; init; } = [];

	/// <summary>
	/// Тип комментария
	/// </summary>
	public CommentType CommentType { get; init; }

	/// <summary>
	/// Способ добавления комментария
	/// </summary>
	public CommentTransportType TransportType { get; init; }

	/// <summary>
	/// Список пользователей, которые призваны в комментарии.
	/// </summary>
	public Collection<string> Summonees { get; init; } = [];

	/// <summary>
	/// Список рассылок, которые призваны в комментарии.
	/// </summary>
	public Collection<string> MaillistSummonees { get; init; } = [];
}
