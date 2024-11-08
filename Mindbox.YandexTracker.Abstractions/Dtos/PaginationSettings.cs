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