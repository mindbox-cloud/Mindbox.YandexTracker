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

public record CustomFieldsRequest
{
	[JsonExtensionData]
	[JsonInclude]
	private Dictionary<string, JsonElement> Fields { get; init; } = [];

	/// <remarks>
	/// Необходимо передавать id кастомного поля, из-за того, что локальные поля очереди будут иметь префикс в своем
	/// названии, которое будет совпадать с id
	/// </remarks>
	public bool TryGetCustomField<T>(string customFieldId, out T? value)
	{
		ArgumentException.ThrowIfNullOrEmpty(customFieldId);
		return CustomFieldsHelper.TryGetCustomField(Fields, customFieldId, out value);
	}

	public T GetCustomField<T>(string customFieldId)
	{
		ArgumentException.ThrowIfNullOrEmpty(customFieldId);
		return CustomFieldsHelper.GetCustomField<T>(Fields, customFieldId);
	}

	/// <remarks>
	/// Необходимо передавать id кастомного поля, из-за того, что локальные поля очереди будут иметь префикс в своем
	/// названии, которое будет совпадать с id
	/// </remarks>
	public void SetCustomField<T>(string customFieldId, T value)
	{
		ArgumentException.ThrowIfNullOrEmpty(customFieldId);
		CustomFieldsHelper.SetCustomField(Fields, customFieldId, value);
	}
}

public record CustomFieldsResponse
{
	[JsonExtensionData]
	[JsonInclude]
	private Dictionary<string, JsonElement> Fields { get; init; } = [];

	/// <remarks>
	/// Необходимо передавать id кастомного поля, из-за того, что локальные поля очереди будут иметь префикс в своем
	/// названии, которое будет совпадать с id
	/// </remarks>
	public bool TryGetCustomField<T>(string customFieldId, out T? value)
	{
		ArgumentException.ThrowIfNullOrEmpty(customFieldId);
		return CustomFieldsHelper.TryGetCustomField(Fields, customFieldId, out value);
	}

	public T GetCustomField<T>(string customFieldId)
	{
		ArgumentException.ThrowIfNullOrEmpty(customFieldId);
		return CustomFieldsHelper.GetCustomField<T>(Fields, customFieldId);
	}
}