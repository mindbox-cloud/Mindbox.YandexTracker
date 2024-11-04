using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Mindbox.YandexTracker;

internal static class CustomFieldsHelper
{
	public static T? GetCustomField<T>(IReadOnlyDictionary<string, JsonElement> fields, string customFieldId)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(customFieldId);

		if (fields.TryGetValue(customFieldId, out var value))
		{
			return value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined
				? default
				: value.Deserialize<T?>();
		}

		throw new KeyNotFoundException($"Key '{customFieldId}' not found in custom fields.");
	}

	public static void SetCustomField<T>(IDictionary<string, JsonElement> fields, string customFieldId, T value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(customFieldId);

		fields[customFieldId] = JsonSerializer.SerializeToElement(value);
	}
}