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

using System;
using System.Collections.Generic;

namespace Mindbox.YandexTracker;

/// <summary>
/// Запрос на создание новой задачи.
/// </summary>
public record CreateIssueRequest : CustomFieldsRequest
{
	/// <summary>
	/// Ключ очереди.
	/// </summary>
	public required string Queue { get; init; }

	/// <summary>
	/// Название задачи.
	/// </summary>
	public required string Summary { get; init; }

	/// <summary>
	/// Идентификатор типа задачи. Тип задачи должен присутствовать в очереди.
	/// </summary>
	/// <remarks>
	/// Если тип не указан, то используется тип задачи, который выбран для очереди по умолчанию.
	/// </remarks>
	public long? Type { get; init; }

	/// <summary>
	/// Дедлайн.
	/// </summary>
	public DateOnly? DueDate { get; init; }

	/// <summary>
	/// Описание задачи.
	/// </summary>
	public string? Description { get; init; }

	/// <summary>
	/// Дата начала.
	/// </summary>
	public DateOnly? Start { get; init; }

	/// <summary>
	/// Дата окончания.
	/// </summary>
	public DateOnly? End { get; init; }

	public string? Parent { get; init; }

	/// <summary>
	/// Логин или идентификатор исполнителя.
	/// </summary>
	public string? Assignee { get; init; }

	/// <summary>
	/// Идентификатор приоритета.
	/// </summary>
	/// <remarks>
	///Если приоритет не указан, то используется приоритет, который выбран для очереди по умолчанию.
	/// </remarks>
	public Priority? Priority { get; init; }

	public IReadOnlyCollection<string>? Tags { get; init; }

	/// <summary>
	/// Идентификатор проекта, к которому относится задача.
	/// </summary>
	public int? Project { get; init; }

	public IReadOnlyCollection<string>? Sprint { get; init; }

	/// <summary>
	/// Поле с уникальным значением, позволяющее предотвратить создание дубликатов задач.
	/// </summary>
	/// <remarks>
	/// При повторной попытке создать задачу с тем же значением данного параметра дубликат создан не будет,
	/// а ответ будет содержать ошибку с кодом 409.
	/// </remarks>
	public string? Unique { get; init; }

	/// <summary>
	/// Значение параметра Story Points.
	/// </summary>
	public double? StoryPoints { get; init; }

	public string? Author { get; init; }

	public string? QaEngineer { get; init; }

	/// <summary>
	/// Массив с идентификаторами или логинами пользователей, перечисленных в поле Доступ.
	/// </summary>
	public IReadOnlyCollection<string>? Access { get; init; }

	public int? PossibleSpam { get; init; }

	public IReadOnlyCollection<string>? AttachmentIds { get; init; }

	public IReadOnlyCollection<string>? Sprint { get; init; }

	public Priority? Priority { get; init; }

	public string? Assignee { get; init; }

	public IReadOnlyCollection<string>? Tags { get; init; }

	public IReadOnlyCollection<CreateChecklistRequest>? ChecklistItems { get; init; }

	/// <summary>
	/// Идентификаторы или логины наблюдателей задачи.
	/// </summary>
	public IReadOnlyCollection<string>? Followers { get; init; }
}
