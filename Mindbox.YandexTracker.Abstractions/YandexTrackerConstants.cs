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

using System.Text.Json;
using System.Text.Json.Serialization;
using Mindbox.YandexTracker.JsonConverters;

namespace Mindbox.YandexTracker;

public static class YandexTrackerConstants
{
	/// <summary>
	/// Формат даты и времени, который принимает API Яндекс.Трекера.
	/// </summary>
	public static readonly string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffzzz";

	/// <summary>
	/// Корректные настройки для сериализации/десериализации JSON для API Яндекс.Трекера.
	/// </summary>
	public static readonly JsonSerializerOptions YandexTrackerJsonSerializerOptions = new()
	{
		Converters =
		{
			// у этиъ 2 enum'ов не camelCase, а какая-то своя фигня в Трекере
			// (например, "ru.yandex.startrek.core.fields.UserFieldType" или snake_case)
			new EnumWithEnumMemberAttributeJsonConverter<QueueLocalFieldType>(),
			new EnumWithEnumMemberAttributeJsonConverter<ProjectEntityStatus>(),
			// а остальные enum'ы - camelCase
			new JsonStringEnumConverter(namingPolicy: JsonNamingPolicy.CamelCase),
			new YandexDateTimeJsonConverter(),
			new YandexNullableDateTimeJsonConverter()
		},
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true
	};
}