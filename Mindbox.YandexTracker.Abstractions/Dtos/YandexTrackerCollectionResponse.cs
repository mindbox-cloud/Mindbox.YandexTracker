using System.Collections.Generic;

namespace Mindbox.YandexTracker;

/// <summary>
/// Представляет ответ от Yandex Tracker, содержащий коллекцию элементов и информацию о пагинации.
/// </summary>
/// <typeparam name="TItem">Тип элементов, содержащихся в ответе.</typeparam>
public record YandexTrackerCollectionResponse<TItem>
{
	/// <summary>
	/// Начальный номер страницы, с которой начато извлечение данных.
	/// Равняется значению <see cref="PaginationSettings.StartPage"/>
	/// </summary>
	public int StartPage { get; init; }

	/// <summary>
	/// Количество страниц, которые были извлечены в текущем ответе.
	/// Равняется значению <see cref="PaginationSettings.MaxPageRequestCount"/> или <see cref="TotalPages"/>
	/// </summary>
	public int FetchedPages { get; init; }

	/// <summary>
	/// Общее количество страниц, доступных для запроса.
	/// Значение берется из хедера X-Total-Pages.
	/// </summary>
	public int TotalPages { get; init; }

	/// <summary>
	/// Коллекция элементов типа <typeparamref name="TItem"/>, содержащихся в текущем ответе.
	/// </summary>
	public IReadOnlyList<TItem> Values { get; init; } = [];
}
