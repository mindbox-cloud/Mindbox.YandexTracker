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
using System.Globalization;
using System.Text;

namespace Mindbox.YandexTracker;

internal static class StringExtensions
{
	/// <summary>
	/// Преобразует enum в строку в camelCase нотации.
	/// </summary>
	/// <remarks>
	/// Потому что в API Яндекс.Трекера требует camelCase нотации и чувствителен к регистру.
	/// </remarks>
	internal static string ToCamelCase(this Enum value)
	{
		return value.ToString().ToCamelCase();
	}

	/// <summary>
	/// Преобразует флаг в строку для query/route для Яндекс.Трекера.
	/// </summary>
	/// <param name="flag">Флаг.</param>
	/// <param name="allValueOption">Значение, когда выбраны все варианты флага</param>
	/// <param name="excludedValues">Значение, которое нужно исключить (например, None).</param>
	/// <typeparam name="T"></typeparam>
	/// <returns>Строка со значениями enum, разделенными запятой, в camelCase нотации.</returns>
	public static string ToYandexQueryString<T>(this T flag, T? allValueOption, T? excludedValues)
		where T : struct, Enum
	{
		// если выбран вариант Все/All, то возвращаем его
		if (allValueOption != null && flag.HasFlag(allValueOption.Value))
		{
			return allValueOption.Value.ToString().ToCamelCase();
		}

		// если нет, то выберем все варианты, кроме исключенных
		var filteredEnumValues = new List<string>();
		foreach (var checkedFlag in Enum.GetValues<T>())
		{
			if (excludedValues != null && excludedValues.Value.HasFlag(checkedFlag)) continue;

			if (!flag.HasFlag(checkedFlag)) continue;

			filteredEnumValues.Add(checkedFlag.ToString().ToCamelCase());
		}

		return string.Join(",", filteredEnumValues);
	}

	/// <summary>
	/// Преобразует строку в camelCase нотацию.
	/// </summary>
	/// <remarks>
	/// Потому что API Яндекс.Трекера требует camelCase нотацию и чувствителен к регистру.
	/// Не учитывает разделители слов (пробелы, и прочее).
	/// </remarks>
	internal static string ToCamelCase(this string str)
	{
		if (string.IsNullOrEmpty(str)) { return str; }

		var camelCaseString = new StringBuilder();

		for (var i = 0; i < str.Length; i++)
		{
			var letter = str[i];

			if (i == 0)
			{
				camelCaseString.Append(CultureInfo.InvariantCulture.TextInfo.ToLower(letter));
			}
			else
			{
				camelCaseString.Append(letter);
			}
		}

		return camelCaseString.ToString();

	}
}
