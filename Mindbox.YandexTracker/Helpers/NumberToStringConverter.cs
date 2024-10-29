using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker.Helpers;

internal class NumberToStringConverter : JsonConverter<string>
{
	public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType is JsonTokenType.Number)
		{
			return reader.GetInt64().ToString(provider: CultureInfo.InvariantCulture);
		}

		if (reader.TokenType is JsonTokenType.String)
		{
			return reader.GetString();
		}

		throw new JsonException("Expected a number or string.");
	}

	public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value);
	}
}