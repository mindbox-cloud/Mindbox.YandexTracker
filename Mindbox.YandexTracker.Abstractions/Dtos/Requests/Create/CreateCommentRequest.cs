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
/// Запрос на создание комментария к задаче.
/// </summary>
public record CreateCommentRequest
{
	/// <summary>
	/// Текст комментария.
	/// </summary>
	public required string Text { get; init; }

	/// <summary>
	/// Список идентификаторов вложений, прикрепленных к комментарию.
	/// </summary>
	public IReadOnlyCollection<string>? AttachmentIds { get; init; }

	/// <summary>
	/// Идентификаторы или логины призванных пользователей.
	/// </summary>
	public IReadOnlyCollection<string>? Summonees { get; init; }

	/// <summary>
	/// Список рассылок, призванных в комментарии.
	/// </summary>
	public IReadOnlyCollection<string>? MaillistSummonees { get; init; }
}