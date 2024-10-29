using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

[assembly: InternalsVisibleTo("Mindbox.YandexTracker")]

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
	/// Дата и время последнего добавленного комментария в UTC.
	/// </summary>
	public DateTime? LastCommentUpdatedAtUtc { get; init; }

	/// <summary>
	/// Название задачи
	/// </summary>
	public string Summary { get; init; } = default!;

	/// <summary>
	/// Ключ родительской задачи
	/// </summary>
	public string? ParentKey { get; init; }

	/// <summary>
	/// Информация о последнем сотруднике, изменявшим задачу
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
	public DateTime CreatedAtUtc { get; init; }

	/// <summary>
	/// Массив с информацией об альтернативных ключах задачи
	/// </summary>
	public Collection<string> Aliases { get; init; } = [];

	/// <summary>
	/// Спринты, в которые добавлена задача.
	/// </summary>
	public Collection<string> Sprints { get; init; } = [];

	/// <summary>
	/// Пользователи, наблюдающие за задачей.
	/// </summary>
	public Collection<UserShortInfo> Followers { get; init; } = [];

	/// <summary>
	/// Информация о создателе задачи
	/// </summary>
	public UserShortInfo CreatedBy { get; init; } = null!;

	/// <summary>
	/// Количество голосов за задачу
	/// </summary>
	public int Votes { get; init; }

	/// <summary>
	/// Исполнитель задачи.
	/// </summary>
	public UserShortInfo? Assignee { get; init; }

	/// <summary>
	/// Автор задачи.
	/// </summary>
	public UserShortInfo? Author { get; init; }

	/// <summary>
	/// Информация о проекте задачи
	/// </summary>
	public string? Project { get; init; }

	/// <summary>
	/// Ключ очереди задачи
	/// </summary>
	public required string QueueKey { get; init; } = null!;

	/// <summary>
	/// Дата и время последнего обновления задачи в UTC.
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
	/// <remarks>
	/// Значение может меняться в зависимости от пользователя, от имени которого выполняется запрос.
	/// </remarks>
	public bool IsFavorite { get; init; }

	/// <summary>
	/// Дата начала работы над задачей.
	/// </summary>
	public DateOnly? Start { get; init; }

	/// <summary>
	/// Текстовые метки, которые облегчают поиск и сортировку задач.
	/// </summary>
	public IReadOnlyCollection<string> Tags { get; init; } = [];

	/// <summary>
	/// Кастомные поля задачи
	/// </summary>
	/// <remarks>
	/// Необходимость JToken заключается в том, что мы не знаем какое объект придет к нам с сервера под заданным ключем.
	/// Чтобы не поломать работу было принято использовать JToken и ограничить доступ к словарю через GetCustomField и
	/// SetCustomField
	/// </remarks>
	internal Dictionary<string, JToken?> CustomFields { get; init; } = [];

	/// <remarks>
	/// Необходимо передавать id кастомного поля, из-за того, что локальные поля очереди будут иметь префикс в своем
	/// названии, которое будет совпадать с id
	/// </remarks>
	public T? GetCustomField<T>(string customFieldId)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(customFieldId);

		if (CustomFields.TryGetValue(customFieldId, out var value))
		{
			return value is null
				? default
				: value.ToObject<T>();
		}

		throw new KeyNotFoundException($"Key '{customFieldId}' not found in custom fields.");
	}

	/// <remarks>
	/// Необходимо передавать id кастомного поля, из-за того, что локальные поля очереди будут иметь префикс в своем
	/// названии, которое будет совпадать с id
	/// </remarks>
	public void SetCustomField<T>(string customFieldId, T value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(customFieldId);

		if (value is null)
			CustomFields[customFieldId] = null;
		else
			CustomFields[customFieldId] = JToken.FromObject(value);
	}

	public IReadOnlyList<string> GetCustomFieldsKeys()
	{
		return CustomFields
			.Select(pair => pair.Key)
			.ToList();
	}
}