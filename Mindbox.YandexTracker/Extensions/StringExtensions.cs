using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Mindbox.YandexTracker;

internal static class StringExtensions
{
	public static string? TrimAndMakeNullIfEmpty(this string value)
	{
		return !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;
	}

	/// <summary>
	/// Преобразует название типа проекта в строку в camelCase нотации.
	/// </summary>
	/// <remarks>
	/// Потому что в API Яндекс.Трекера требует camelCase нотации и чувствителен к регистру.
	/// </remarks>
	public static string ToCamelCase(this ProjectEntityType value)
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
		foreach (var checkedFlag in Enum.GetValues(typeof(T)).Cast<T>())
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
	private static string ToCamelCase(this string str)
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
