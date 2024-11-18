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
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker.JsonConverters;

/// <summary>
/// Кастомный конвертер для сериализации/десериализации даты и времени в формате Yandex'а.
/// </summary>
internal class YandexDateTimeJsonConverter : JsonConverter<DateTime>
{

	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.String) throw new JsonException("Expected date string");

		var dateString = reader.GetString();
		if (DateTime.TryParseExact(
				dateString,
				YandexTrackerConstants.DateTimeFormat,
				CultureInfo.InvariantCulture,
				DateTimeStyles.None, out DateTime date))
		{
			return date;
		}

		throw new JsonException($"Invalid date format: {dateString}");
	}

	public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString(YandexTrackerConstants.DateTimeFormat, CultureInfo.InvariantCulture));
	}
}