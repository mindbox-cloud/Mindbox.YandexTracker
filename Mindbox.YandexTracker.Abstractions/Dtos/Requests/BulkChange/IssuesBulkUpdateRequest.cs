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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Запрос на массовое изменения задач.
/// </summary>
public record IssuesBulkUpdateRequest
{
	/// <summary>
	/// Ключи задач, которые нужно изменить.
	/// </summary>
	public required IReadOnlyList<string> Issues { get; init; }

	/// <summary>
	/// Изменяемые значения.
	/// </summary>
	public required IssuesBulkUpdateValues Values { get; init; }
}

/// <summary>
/// Изменяемые значения задач в рамках массового изменения <see cref="IssuesBulkUpdateRequest"/>.
/// </summary>
public record IssuesBulkUpdateValues
{
	[JsonExtensionData]
	[JsonInclude]
	private Dictionary<string, JsonElement> Values { get; init; } = [];

	/// <summary>
	/// Устанавливает значение, которое нужно изменить в задачах.
	/// </summary>
	/// <remarks>
	/// ВАЖНО! Нельзя тут указывать локальные поля очереди.
	/// </remarks>
	public void SetValue<T>(string fieldId, T value)
	{
		ArgumentException.ThrowIfNullOrEmpty(fieldId);
		CustomFieldsHelper.SetCustomField(Values, fieldId, value);
	}
}