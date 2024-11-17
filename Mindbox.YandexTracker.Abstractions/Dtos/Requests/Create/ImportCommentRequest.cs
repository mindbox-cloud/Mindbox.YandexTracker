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
/// Запрос на импорт комментария к задаче.
/// </summary>
/// <remarks>
/// Должен вызываться только администратором системы.
/// В отличие от <see cref="CreateCommentRequest"/>, позволяет указать дату создания комментария,
/// а также в качестве создателя задачи указать любого пользователя.
/// </remarks>
public record ImportCommentRequest
{
	/// <summary>
	/// Текст комментария.
	/// </summary>
	public required string Text { get; init; }

	/// <summary>
	/// Автор комментария.
	/// </summary>
	public required string CreatedBy { get; init; }

	/// <summary>
	/// Дата и время создания комментария.
	/// </summary>
	public DateTime CreatedAt { get; init; }

	/// <summary>
	/// Пользователь, который последний раз обновлял комментарий.
	/// </summary>
	public string? UpdatedBy { get; init; }

	/// <summary>
	/// Дата и время последнего обновления комментария.
	/// </summary>
	public DateTime? UpdatedAt { get; init; }
}