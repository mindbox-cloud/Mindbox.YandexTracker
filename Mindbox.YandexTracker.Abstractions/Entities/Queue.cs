using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

// Queue - зарезервированное слово, идентификатор не должен на него оканчиваться, CA1711 на это ругается
// Но название норм по бизнесу, поэтому повесим прагму.
#pragma warning disable CA1711
/// <summary>
/// Очередь — это пространство для задач, объединенных общим процессом или продуктом.
/// </summary>
public sealed record Queue
#pragma warning restore CA1711
{
	/// <summary>
	/// Идентификатор очереди
	/// </summary>
	public string Id { get; init; } = null!;

	/// <summary>
	/// Ключ очереди
	/// </summary>
	public required string Key { get; init; }

	/// <summary>
	/// Название очереди
	/// </summary>
	public required string Name { get; init; }

	/// <summary>
	/// Информация об ответственном сотруднике
	/// </summary>
	public required UserShortInfo Lead { get; init; }

	/// <summary>
	/// Описание очереди
	/// </summary>
	public string? Description { get; init; }

	/// <summary>
	/// Автоматически назначить исполнителя для новых задач очереди:
	/// true— назначить;
	/// false— не назначать.
	/// </summary>
	public bool AssignAuto { get; init; }

	/// <summary>
	/// Тип задачи по умолчанию
	/// </summary>
	public required IssueType DefaultType { get; init; }

	/// <summary>
	/// Приоритет по умолчанию
	/// </summary>
	public Priority DefaultPriority { get; init; }

	/// <summary>
	/// Массив с информацией об участниках команды очереди
	/// </summary>
	public Collection<UserShortInfo> TeamUsers { get; init; } = [];

	/// <summary>
	/// Массив с информацией о типах задач очереди
	/// </summary>
	public Collection<IssueType> IssueTypes { get; init; } = [];

	/// <summary>
	/// Массив с настройками задач очереди
	/// </summary>
	public Collection<IssueTypeConfig> IssueTypesConfig { get; init; } = [];

	/// <summary>
	/// Список жизненных циклов очереди и их типов задач
	/// </summary>
	public Collection<IssueType> Workflows { get; init; } = [];

	/// <summary>
	/// Признак возможности голосования за задачи
	/// </summary>
	public bool DenyVoting { get; init; }
}