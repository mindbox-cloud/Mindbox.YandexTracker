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

namespace Mindbox.YandexTracker;

/// <summary>
/// Запрос на импорт задачи в Яндекс.Трекер.
/// </summary>
/// <remarks>
/// Должен вызываться только администратором системы.
/// В отличие от <see cref="CreateIssueRequest"/>, позволяет указать дату создания из прошлого,
/// а также в качестве создателя задачи указать любого пользователя.
/// </remarks>
public record ImportIssueRequest : CreateIssueRequest
{
	/// <summary>
	/// Дата и время создания задачи.
	/// </summary>
	/// <remarks>
	/// Не может быть позже текущего времени.
	/// </remarks>
	public DateTime CreatedAt { get; init; }

	/// <summary>
	/// Логин или идентификатор автора задачи.
	/// </summary>
	public required string CreatedBy { get; init; }

	/// <summary>
	/// Дата и время последнего изменения задачи.
	/// </summary>
	/// <remarks>
	/// Не может быть позже текущего времени.
	/// Параметр указывается только вместе с <see cref="UpdatedBy"/>.
	/// </remarks>
	public DateTime? UpdatedAt { get; init; }

	/// <summary>
	/// Логин или идентификатор пользователя, который последний редактировал задачу.
	/// </summary>
	/// <remarks>
	/// Параметр указывается только вместе с <see cref="UpdatedAt"/>.
	/// </remarks>
	public string? UpdatedBy { get; init; }

	/// <summary>
	/// Дата и время проставления резолюции.
	/// </summary>
	/// <remarks>
	/// Вы можете указать любое значение в интервале времени от создания до последнего изменения задачи.
	/// Параметр указывается только вместе с <see cref="ResolvedBy"/>.
	/// </remarks>
	public DateTime? ResolvedAt { get; init; }

	/// <summary>
	/// Логин или идентификатор пользователя, который последний редактировал задачу.
	/// </summary>
	/// <remarks>
	/// Параметр указывается только вместе с <see cref="ResolvedAt"/> и <see cref="Resolution"/>.
	/// </remarks>
	public string? ResolvedBy { get; init; }

	/// <summary>
	/// Идентификатор резолюции задачи.
	/// </summary>
	/// <remarks>
	/// Параметр указывается только вместе с <see cref="ResolvedAt"/> и <see cref="ResolvedBy"/>.
	/// </remarks>
	public long? Resolution { get; init; }
}
