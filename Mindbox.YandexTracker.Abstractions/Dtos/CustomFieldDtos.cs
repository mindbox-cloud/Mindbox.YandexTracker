using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

public record CustomFieldsRequest
{
	[JsonExtensionData]
	public Dictionary<string, JsonElement> Fields { get; init; } = [];

	/// <remarks>
	/// Необходимо передавать id кастомного поля, из-за того, что локальные поля очереди будут иметь префикс в своем
	/// названии, которое будет совпадать с id
	/// </remarks>
	public T? GetCustomField<T>(string customFieldId)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(customFieldId);
		return CustomFieldsHelper.GetCustomField<T>(Fields, customFieldId);
	}

	/// <remarks>
	/// Необходимо передавать id кастомного поля, из-за того, что локальные поля очереди будут иметь префикс в своем
	/// названии, которое будет совпадать с id
	/// </remarks>
	public void SetCustomField<T>(string customFieldId, T value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(customFieldId);
		CustomFieldsHelper.SetCustomField(Fields, customFieldId, value);
	}
}

public record CustomFieldsResponse
{
	[JsonExtensionData]
	public IDictionary<string, JsonElement> Fields { get; init; } = new Dictionary<string, JsonElement>();

	/// <remarks>
	/// Необходимо передавать id кастомного поля, из-за того, что локальные поля очереди будут иметь префикс в своем
	/// названии, которое будет совпадать с id
	/// </remarks>
	public T? GetCustomField<T>(string customFieldId)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(customFieldId);
		return CustomFieldsHelper.GetCustomField<T>(Fields, customFieldId);
	}
}