using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker.JsonConverters;

internal class YandexDateTimeConverter : JsonConverter<DateTime>
{
	private const string DateFormat = "yyyy-MM-ddTHH:mm:ss.fffzzz"; // Adjust this to match your JSON date format

	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.String) throw new JsonException("Expected date string");

		var dateString = reader.GetString();
		if (DateTime.TryParseExact(
				dateString,
				DateFormat,
				CultureInfo.InvariantCulture,
				DateTimeStyles.None, out DateTime date))
		{
			return date;
		}

		throw new JsonException($"Invalid date format: {dateString}");
	}

	public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
	}
}