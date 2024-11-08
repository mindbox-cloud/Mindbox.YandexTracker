// Copyright 2024 Mindbox Ltd
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
