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
/// Базовый класс для ответа на запрос массового изменения задач.
/// </summary>
public abstract record BulkChangeResponseBase
{
	/// <summary>
	/// Идентификатор операции массового редактирования.
	/// </summary>
	public required string Id { get; init; }

	/// <summary>
	/// Дата и время создания операции массового редактирования.
	/// </summary>
	public DateTime CreatedAt { get; init; }

	/// <summary>
	/// Инициатор массового редактирования.
	/// </summary>
	public required UserShortInfoDto CreatedBy { get; init; }

	/// <summary>
	/// Статус операции массового редактирования.
	/// </summary>
	public required string Status { get; init; }

	/// <summary>
	/// Описание статуса операции массового редактирования.
	/// </summary>
	public required string StatusText { get; init; }
}