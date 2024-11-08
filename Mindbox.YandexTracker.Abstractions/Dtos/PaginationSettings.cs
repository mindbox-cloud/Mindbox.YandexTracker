namespace Mindbox.YandexTracker;

/// <summary>
/// Представляет параметры пагинации для управления извлечением данных по страницам.
/// https://yandex.cloud/ru/docs/tracker/common-format#displaying-results
/// </summary>
public record PaginationSettings
{
	/// <summary>
	/// Номер страницы, с которой начинается пагинация.
	/// Нумерация начинается с единицы.
	/// </summary>
	public int StartPage { get; init; } = 1;

	/// <summary>
	/// Задает количество элементов, извлекаемых на одной странице.
	/// 100 - максимальное значение.
	/// </summary>
	public int PerPage { get; init; } = 100;

	/// <summary>
	/// Получает или задает максимальное количество запросов страниц.
	/// Если установлено значение null, то количество запросов страниц не ограничено.
	/// </summary>
	public int? MaxPageRequestCount { get; init; }
}