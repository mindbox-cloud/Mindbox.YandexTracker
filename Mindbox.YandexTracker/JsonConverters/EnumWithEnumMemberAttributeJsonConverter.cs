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
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker.JsonConverters;

/// <summary>
/// Кастомный конвертер для сериализации/десериализации enum'ов с атрибутом EnumMemberAttribute.
/// </summary>
/// <remarks>
/// При конвертации в json значение берется из атрибута. JsonPropertyAttribute атрибут для enum'ов завезут только в .NET 9.
/// </remarks>
internal class EnumWithEnumMemberAttributeJsonConverter<T> : JsonConverter<T> where T: Enum
{
	public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		string? enumString = reader.GetString();
		for (var i = 0; i < typeToConvert.GetFields().Length; i++)
		{
			var field = typeToConvert.GetFields()[i];

			if (field.GetCustomAttribute<EnumMemberAttribute>()?.Value == enumString)
			{
				return (T)field.GetValue(null)!;
			}
		}

		if (enumString == null) throw new JsonException($"Unknown value for {nameof(QueueLocalFieldType)} ({enumString})");

		return (T)Enum.Parse(typeToConvert, enumString, true);
	}

	public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
	{
		var enumMemberAttribute = value.GetType().GetField(value.ToString())?.GetCustomAttribute<EnumMemberAttribute>();
		writer.WriteStringValue(enumMemberAttribute != null ? enumMemberAttribute.Value : value.ToString());
	}
}