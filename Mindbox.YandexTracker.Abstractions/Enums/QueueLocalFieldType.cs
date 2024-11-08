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

using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Тип локального поля очереди
/// </summary>
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