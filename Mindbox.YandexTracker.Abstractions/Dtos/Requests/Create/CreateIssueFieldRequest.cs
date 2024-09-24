using System;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class CreateIssueFieldRequest
{
	[DataMember(Name = "id")]
	public required string Id { get; set; }

	[DataMember(Name = "type")]
	public required FieldType Type { get; set; }

	[DataMember(Name = "category")]
	public required string Category { get; set; }

	[DataMember(Name = "name")]
	public required NameInfoDto Name { get; set; }

	/// <summary>
	/// Порядковый номер в списке полей организации: https://tracker.yandex.ru/admin/fields.
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "order")]
	public int? Order { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "description")]
	public string? Description { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "readonly")]
	public bool? Readonly { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "visible")]
	public bool? Visible { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "hidden")]
	public bool? Hidden { get; set; }

	/// <summary>
	/// Признак возможности указать в поле одновременно несколько значений.
	/// Этот параметр допустимо использовать для полей следующих типов:
	/// <para> ru.yandex.startrek.core.fields.StringFieldType — Текстовое однострочное поле; </para>
	/// <para> ru.yandex.startrek.core.fields.UserFieldType — Имя пользователя; </para>
	/// <para> выпадающий список(см.описание объекта optionsProvider) </para>
	/// </summary>
	[DataMember(EmitDefaultValue = false, Name = "container")]
	public bool? Container { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "optionsProvider")]
	public OptionsProviderDto? OptionsProvider { get; set; }
}

[DataContract]
public class NameInfoDto
{
	[DataMember(Name = "en")]
	public required string En { get; set; }

	[DataMember(Name = "ru")]
	public required string Ru { get; set; }
}

[DataContract]
public class OptionsProviderDto
{
	/// <summary>
	/// Тип выпадающего списка:
	/// <para>FixedListOptionsProvider — список строковых или числовых значений
	/// (для полей с типом StringFieldType или IntegerFieldType); </para>
	/// <para>FixedUserListOptionsProvider — список пользователей(для полей с типом UserFieldType). </para>
	/// </summary>
	[DataMember(Name = "type")]
	public required string Type { get; set; }

	[DataMember(Name = "values")]
#pragma warning disable IDE0301
#pragma warning disable CA1819
	public string[] Values { get; set; } = Array.Empty<string>();
#pragma warning restore CA1819
#pragma warning restore IDE0301

	[DataMember(EmitDefaultValue = false, Name = "needValidation")]
	public bool? NeedValidation { get; set; }
}