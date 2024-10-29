using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker.Helpers;

internal class DateTimeConverter: JsonConverter<DateTime>
{
	private const string DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss.fffzzz";

	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (DateTime.TryParseExact(
			    reader.GetString(),
			    DateTimeFormat,
			    null,
			    System.Globalization.DateTimeStyles.AdjustToUniversal,
			    out DateTime date))
		{
			return date;
		}
		throw new JsonException("Invalid date format.");
	}

	public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
	}
}