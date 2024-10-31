using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record IssuesFilterDto
{
	/// <summary>
	/// Дата и время последнего добавленного комментария
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "lastCommentUpdatedAt")]
	public DateTime? LastCommentUpdatedAtUtc { get; init; }

	/// <summary>
	/// Название задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "summary")]
	public string? Summary { get; init; }

	/// <summary>
	/// Идентификатор родительской задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "parent")]
	public string? Parent { get; init; }

	/// <summary>
	/// Массив с информацией об альтернативных ключах задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "alliases")]
	public Collection<string>? Aliases { get; init; }

	/// <summary>
	/// Идентификатор последнего сотрудника, изменившего задачу
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "updatedBy")]
	public string? UpdatedBy { get; init; }

	/// <summary>
	/// Описание задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "description")]
	public string? Description { get; init; }

	/// <summary>
	/// Массив с информацией о спринте
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "sprint")]
	public Collection<string>? Sprints { get; init; }

	/// <summary>
	/// Тип задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "type")]
	public GetIssueTypeResponse? Type { get; init; }

	/// <summary>
	/// Приоритет задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "priority")]
	public Priority? Priority { get; init; }

	/// <summary>
	/// Дата и время создания задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "createdAt")]
	public DateTime? CreatedAtUtc { get; init; }

	/// <summary>
	/// Массив идентификаторов наблюдателей задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "followers")]
	public Collection<string>? Followers { get; init; }

	/// <summary>
	/// Идентификатор создателя задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "createdBy")]
	public string? CreatedBy { get; init; }

	/// <summary>
	/// Количество голосов за задачу
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "votes")]
	public int? Votes { get; init; }

	/// <summary>
	/// Идентификатор исполнителя задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "assignee")]
	public string? Assignee { get; init; }

	/// <summary>
	/// Идентификатор проекта задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "project")]
	public string? Project { get; init; }

	/// <summary>
	/// Идентификатор очереди задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "queue")]
	public string? Queue { get; init; }

	/// <summary>
	/// Дата и время последнего обновления задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "updatedAt")]
	public DateTime? UpdatedAtUtc { get; init; }

	/// <summary>
	/// Статус задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "status")]
	public string? Status { get; init; }

	/// <summary>
	/// Предыдущий статус задачи
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "previousStatus")]
	public GetIssueStatusResponse? PreviousStatus { get; init; }

	/// <summary>
	/// Признак избранной задачи:
	/// true — пользователь добавил задачу в избранное;
	/// false — задача не добавлена в избранное.
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "favorite")]
	public bool? IsFavorite { get; init; }

	public override int GetHashCode()
	{
		var hashCodePart1 = HashCode.Combine(
			LastCommentUpdatedAtUtc,
			Summary,
			Parent,
			UpdatedBy,
			Description,
			Type,
			Priority,
			CreatedAtUtc);

		var hashCodePart2 = HashCode.Combine(
			CreatedBy,
			Votes,
			Assignee,
			Project,
			UpdatedAtUtc,
			Status,
			PreviousStatus,
			IsFavorite);

		var collectionHashCode = 0;

		foreach (var alias in Aliases ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, alias.GetHashCode(StringComparison.InvariantCulture));
		}

		foreach (var sprint in Sprints ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, sprint.GetHashCode(StringComparison.InvariantCulture));
		}

		foreach (var follower in Followers ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, follower.GetHashCode(StringComparison.InvariantCulture));
		}

		return HashCode.Combine(hashCodePart1, hashCodePart2, collectionHashCode, Queue);
	}
}
