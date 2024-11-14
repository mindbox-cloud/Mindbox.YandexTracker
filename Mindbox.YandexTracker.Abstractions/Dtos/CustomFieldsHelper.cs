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

namespace Mindbox.YandexTracker;

internal static class CustomFieldsHelper
{
	public static bool TryGetCustomField<T>(
		IDictionary<string, JsonElement> fields,
		string customFieldId,
		out T? customField)
	{
		ArgumentException.ThrowIfNullOrEmpty(customFieldId);

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
		ArgumentException.ThrowIfNullOrEmpty(customFieldId);

		fields[customFieldId] = JsonSerializer.SerializeToElement(value);
	}
}