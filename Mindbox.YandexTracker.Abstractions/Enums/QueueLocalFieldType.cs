using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Тип локального поля очереди
/// </summary>
[DataContract]
public enum QueueLocalFieldType
{
	/// <summary>
	/// Дата
	/// </summary>
	[EnumMember(Value = "ru.yandex.startrek.core.fields.DateFieldType")]
	DateFieldType,

	/// <summary>
	/// Дата/Время
	/// </summary>
	[EnumMember(Value = "ru.yandex.startrek.core.fields.DateTimeFieldType")]
	DateTimeFieldType,

	/// <summary>
	/// Текстовое однострочное поле
	/// </summary>
	[EnumMember(Value = "ru.yandex.startrek.core.fields.StringFieldType")]
	StringFieldType,

	/// <summary>
	/// Текстовое многострочное поле
	/// </summary>
	[EnumMember(Value = "ru.yandex.startrek.core.fields.TextFieldType")]
	TextFieldType,

	/// <summary>
	/// Дробное число
	/// </summary>
	[EnumMember(Value = "ru.yandex.startrek.core.fields.FloatFieldType")]
	FloatFieldType,

	/// <summary>
	/// Целое число
	/// </summary>
	[EnumMember(Value = "ru.yandex.startrek.core.fields.IntegerFieldType")]
	IntegerFieldType,

	/// <summary>
	/// Имя пользователя
	/// </summary>
	[EnumMember(Value = "ru.yandex.startrek.core.fields.UserFieldType")]
	UserFieldType,

	/// <summary>
	/// Ссылка
	/// </summary>
	[EnumMember(Value = "ru.yandex.startrek.core.fields.UriFieldType")]
	UriFieldType
}