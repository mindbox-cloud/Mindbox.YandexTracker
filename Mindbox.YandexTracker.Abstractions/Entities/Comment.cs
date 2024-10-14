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
	/// Информация о создателе комментария
	/// </summary>
	public UserShortInfo CreatedBy { get; init; } = null!;

	/// <summary>
	/// Информация о сотруднике, внесшем последнее изменение в комментарий
	/// </summary>
	public UserShortInfo? UpdatedBy { get; init; }

	/// <summary>
	/// Дата и время создания комментария
	/// </summary>
	public DateTime CreatedAtUtc { get; init; }

	/// <summary>
	/// Дата и время обновления комментария
	/// </summary>
	public DateTime? UpdatedAtUtc { get; init; }

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
	/// Массив с информацией о пользователях, которые призваны в комментарии
	/// </summary>
	public Collection<string> Summonees { get; init; } = [];

	/// <summary>
	/// Массив с информацией о рассылках, которые призваны в комментарии
	/// </summary>
	public Collection<string> MaillistSummonees { get; init; } = [];
}
