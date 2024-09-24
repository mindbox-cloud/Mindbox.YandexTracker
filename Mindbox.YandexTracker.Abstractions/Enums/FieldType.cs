using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public enum FieldType
{
	[EnumMember(Value = "ru.yandex.startrek.core.fields.DateFieldType")]
	DateFieldType,

	[EnumMember(Value = "ru.yandex.startrek.core.fields.DateTimeFieldType")]
	DateTimeFieldType,

	[EnumMember(Value = "ru.yandex.startrek.core.fields.StringFieldType")]
	StringFieldType,

	[EnumMember(Value = "ru.yandex.startrek.core.fields.TextFieldType")]
	TextFieldType,

	[EnumMember(Value = "ru.yandex.startrek.core.fields.FloatFieldType")]
	FloatFieldType,

	[EnumMember(Value = "ru.yandex.startrek.core.fields.IntegerFieldType")]
	IntegerFieldType,

	[EnumMember(Value = "ru.yandex.startrek.core.fields.UserFieldType")]
	UserFieldType,

	[EnumMember(Value = "ru.yandex.startrek.core.fields.UriFieldType")]
	UriFieldType
}
