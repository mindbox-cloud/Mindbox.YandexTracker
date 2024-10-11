using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

/// <summary>
/// Комментарий
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
	/// Объект с информацией о создателе комментария
	/// </summary>
	public UserShortInfo CreatedBy { get; init; } = null!;

	/// <summary>
	/// Объект с информацией о сотруднике, внесшем последнее изменение в комментарий
	/// </summary>
	public UserShortInfo? UpdatedBy { get; init; }

	/// <summary>
	/// Дата и время создания комментария
	/// </summary>
	public DateTime CreatedAt { get; init; }

	/// <summary>
	/// Дата и время обновления комментария
	/// </summary>
	public DateTime? UpdatedAt { get; init; }

	/// <summary>
	/// Вложения
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
	/// Блок с информацией о пользователях, которые призваны в комментарии
	/// </summary>
	public Collection<string> Summonees { get; init; } = [];

	/// <summary>
	/// Блок с информацией о рассылках, которые призваны в комментарии
	/// </summary>
	public Collection<string> MaillistSummonees { get; init; } = [];
}
