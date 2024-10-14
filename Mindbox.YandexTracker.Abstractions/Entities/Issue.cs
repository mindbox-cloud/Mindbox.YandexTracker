using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

/// <summary>
/// Задача
/// </summary>
public sealed record Issue
{
	/// <summary>
	/// Ключ задачи
	/// </summary>
	public string Key { get; init; } = null!;

	/// <summary>
	/// Дата и время последнего добавленного комментария
	/// </summary>
	public DateTime? LastCommentUpdatedAt { get; init; }

	/// <summary>
	/// Название задачи
	/// </summary>
	public string Summary { get; init; } = default!;

	/// <summary>
	/// Объект с информацией о родительской задаче
	/// </summary>
	public string? Parent { get; init; }

	/// <summary>
	/// Объект с информацией о последнем сотруднике, изменявшим задачу
	/// </summary>
	public UserShortInfo UpdatedBy { get; init; } = null!;

	/// <summary>
	/// Описание задачи
	/// </summary>
	public string? Description { get; init; }

	/// <summary>
	/// Тип задачи
	/// </summary>
	public IssueType Type { get; init; } = null!;

	/// <summary>
	/// Приоритет задачи
	/// </summary>
	public Priority Priority { get; init; }

	/// <summary>
	/// Дата и время создания задачи
	/// </summary>
	public DateTime CreatedAt { get; init; }

	/// <summary>
	/// Массив с информацией об альтернативных ключах задачи
	/// </summary>
	public Collection<string> Aliases { get; init; } = [];

	/// <summary>
	/// Массив объектов с информацией о спринте
	/// </summary>
	public Collection<string> Sprints { get; init; } = [];

	/// <summary>
	/// Массив объектов с информацией о наблюдателях задачи
	/// </summary>
	public Collection<UserShortInfo> Followers { get; init; } = [];

	/// <summary>
	/// Объект с информацией о создателе задачи
	/// </summary>
	public UserShortInfo CreatedBy { get; init; } = null!;

	/// <summary>
	/// Количество голосов за задачу
	/// </summary>
	public int Votes { get; init; }

	/// <summary>
	/// Объект с информацией об исполнителе задачи
	/// </summary>
	public UserShortInfo? Assignee { get; init; }

	/// <summary>
	/// Объект с информацией о авторе задачи
	/// </summary>
	public UserShortInfo? Author { get; init; }

	/// <summary>
	/// Объект с информацией о проекте задачи
	/// </summary>
	public string? Project { get; init; }

	/// <summary>
	/// Очередь задачи
	/// </summary>
	public required string Queue { get; init; } = null!;

	/// <summary>
	/// Дата и время последнего обновления задачи
	/// </summary>
	public DateTime UpdatedAt { get; init; }

	/// <summary>
	/// Cтатус задачи
	/// </summary>
	public IssueStatus Status { get; init; } = null!;

	/// <summary>
	/// Предыдущий статус задачи
	/// </summary>
	public IssueStatus? PreviousStatus { get; init; }

	/// <summary>
	/// Признак избранной задачи:
	/// true — пользователь добавил задачу в избранное;
	/// false — задача не добавлена в избранное.
	/// </summary>
	public bool IsFavorite { get; init; }

	/// <summary>
	/// Кастомные поля задачи
	/// </summary>
	public Dictionary<string, object?> CustomFields { get; init; } = [];

	public override int GetHashCode()
	{
		var hashCodePart1 = HashCode.Combine(
			Key,
			LastCommentUpdatedAt,
			Summary,
			Parent,
			UpdatedBy,
			Description,
			Type,
			Priority
		);

		var hashCodePart2 = HashCode.Combine(
			CreatedAt,
			CreatedBy,
			Votes,
			Assignee,
			Author,
			Project,
			Queue,
			UpdatedAt
		);

		var hashCodePart3 = HashCode.Combine(
			Status,
			PreviousStatus,
			IsFavorite
		);

		var collectionHashCode = 0;

		foreach (var alias in Aliases)
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, alias.GetHashCode(StringComparison.InvariantCulture));
		}

		foreach (var sprint in Sprints)
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, sprint.GetHashCode(StringComparison.InvariantCulture));
		}

		foreach (var follower in Followers)
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, follower.GetHashCode());
		}

		foreach (var field in CustomFields)
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, field.Key, field.Value?.GetHashCode());
		}

		return HashCode.Combine(hashCodePart1, hashCodePart2, hashCodePart3, collectionHashCode);
	}
}