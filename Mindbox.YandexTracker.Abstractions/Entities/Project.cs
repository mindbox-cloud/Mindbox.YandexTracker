using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

/// <summary>
/// Проект
/// </summary>
public sealed record Project
{
	/// <summary>
	/// Идентификатор сущности
	/// </summary>
	public string Id { get; init; } = null!;

	/// <summary>
	/// Идентификатор проекта или портфеля
	/// </summary>
	public int ShortId { get; init; }

	/// <summary>
	/// Тип сущности
	/// </summary>
	public ProjectEntityType ProjectType { get; init; }

	/// <summary>
	/// Информация о создателе проекта
	/// </summary>
	public UserShortInfo CreatedBy { get; init; } = null!;

	/// <summary>
	/// Дата и время создания проекта
	/// </summary>
	public DateTime CreatedAtUtc { get; init; }

	/// <summary>
	/// Дата последнего обновления сущности
	/// </summary>
	public DateTime UpdatedAtUtc { get; init; }

	/// <summary>
	/// Название проекта
	/// </summary>
	public string? Summary { get; init; }

	/// <summary>
	/// Описание проекта
	/// </summary>
	public string? Description { get; init; }

	/// <summary>
	/// Информация о авторе проекта
	/// </summary>
	public UserShortInfo? Author { get; init; }

	/// <summary>
	/// Информация об ответственном за проект
	/// </summary>
	public UserShortInfo? Lead { get; init; }

	/// <summary>
	/// Участники проекта
	/// </summary>
	public Collection<UserShortInfo>? TeamUsers { get; init; }

	/// <summary>
	/// Заказчики
	/// </summary>
	public Collection<UserShortInfo>? Clients { get; init; }

	/// <summary>
	/// Наблюдатели
	/// </summary>
	public Collection<UserShortInfo>? Followers { get; init; }

	/// <summary>
	/// Теги
	/// </summary>
	public Collection<string>? Tags { get; init; }

	/// <summary>
	/// Дата начала
	/// </summary>
	public DateTime? StartUtc { get; init; }

	/// <summary>
	/// Дедлайн
	/// </summary>
	public DateTime? EndUtc { get; init; }

	/// <summary>
	/// Доступ
	/// </summary>
	public bool? TeamAccess { get; init; }

	/// <summary>
	/// Статус
	/// </summary>
	public ProjectEntityStatus? Status { get; init; }

	/// <summary>
	/// Кварталы начала и окончания проекта
	/// </summary>
	public Collection<string>? Quarter { get; init; }

	/// <summary>
	/// Идентификаторы чеклистов
	/// </summary>
	public Collection<string>? ChecklistIds { get; init; }

	/// <summary>
	/// Идентификатор родительского проекта
	/// </summary>
	public int? ParentId { get; init; }

	public override int GetHashCode()
	{
		var hashCodePart1 = HashCode.Combine(
			Id,
			ShortId,
			ProjectType,
			CreatedBy,
			CreatedAtUtc,
			UpdatedAtUtc,
			Summary,
			Description);

		var hashCodePart2 = HashCode.Combine(Author, Lead, StartUtc, EndUtc, TeamAccess, Status, ParentId);

		var collectionHashCode = 0;

		foreach (var teamUser in TeamUsers ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, teamUser.GetHashCode());
		}

		foreach (var client in Clients ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, client.GetHashCode());
		}

		foreach (var follower in Followers ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, follower.GetHashCode());
		}

		foreach (var tag in Tags ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, tag.GetHashCode(StringComparison.InvariantCulture));
		}

		foreach (var quarter in Quarter ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, quarter.GetHashCode(StringComparison.InvariantCulture));
		}

		foreach (var checklistId in ChecklistIds ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, checklistId.GetHashCode(StringComparison.InvariantCulture));
		}

		return HashCode.Combine(hashCodePart1, hashCodePart2, collectionHashCode);
	}
}
