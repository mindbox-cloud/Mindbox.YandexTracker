using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Mindbox.YandexTracker;

internal static class CustomFieldsHelper
{
	public static bool TryGetCustomField<T>(
		IDictionary<string, JsonElement> fields,
		string customFieldId,
		out T? customField)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(customFieldId);

		customField = default;
		if (fields.TryGetValue(customFieldId, out var value))
		{
			if (value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
				return false;

			customField = value.Deserialize<T>();
			return true;
		}

		return false;
	}

	public static T GetCustomField<T>(IDictionary<string, JsonElement> fields, string customFieldId)
	{
		return TryGetCustomField<T>(fields, customFieldId, out var value)
			? value!
			: throw new KeyNotFoundException($"Key '{customFieldId}' not found in custom fields.");
	}

	public static void SetCustomField<T>(IDictionary<string, JsonElement> fields, string customFieldId, T value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(customFieldId);

		fields[customFieldId] = JsonSerializer.SerializeToElement(value);
	}
}