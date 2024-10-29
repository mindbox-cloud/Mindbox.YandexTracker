using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker.JsonConverters;

public class QueueLocalFieldTypeConverter : JsonConverter<QueueLocalFieldType>
{
	public override QueueLocalFieldType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		string? enumString = reader.GetString();
		foreach (var field in typeToConvert.GetFields())
		{
			if (field.GetCustomAttribute<EnumMemberAttribute>()?.Value == enumString)
			{
				return (QueueLocalFieldType)field.GetValue(null)!;
			}
		}

		if (enumString == null) throw new JsonException($"Unknown value for {nameof(QueueLocalFieldType)} ({enumString})");

		return (QueueLocalFieldType)Enum.Parse(typeToConvert, enumString, true);
	}

	public override void Write(Utf8JsonWriter writer, QueueLocalFieldType value, JsonSerializerOptions options)
	{
		var enumMemberAttribute = value.GetType().GetField(value.ToString())?.GetCustomAttribute<EnumMemberAttribute>();
		writer.WriteStringValue(enumMemberAttribute != null ? enumMemberAttribute.Value : value.ToString());
	}
}