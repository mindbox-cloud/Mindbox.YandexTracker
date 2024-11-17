using System;

namespace Mindbox.YandexTracker;

/// <summary>
/// Запрос на импорт комментария к задаче.
/// </summary>
/// <remarks>
/// Должен вызываться только администратором системы.
/// В отличие от <see cref="CreateCommentRequest"/>, позволяет указать дату создания комментария,
/// а также в качестве создателя задачи указать любого пользователя.
/// </remarks>
public record ImportCommentRequest
{
	/// <summary>
	/// Текст комментария.
	/// </summary>
	public required string Text { get; init; }

	/// <summary>
	/// Автор комментария.
	/// </summary>
	public required string CreatedBy { get; init; }

	/// <summary>
	/// Дата и время создания комментария.
	/// </summary>
	public DateTime CreatedAt { get; init; }

	/// <summary>
	/// Пользователь, который последний раз обновлял комментарий.
	/// </summary>
	public string? UpdatedBy { get; init; }

	/// <summary>
	/// Дата и время последнего обновления комментария.
	/// </summary>
	public DateTime? UpdatedAt { get; init; }
}