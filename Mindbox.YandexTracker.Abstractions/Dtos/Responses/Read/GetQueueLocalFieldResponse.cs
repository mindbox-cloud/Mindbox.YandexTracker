using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json;

namespace Mindbox.YandexTracker;

public sealed record GetQueueLocalFieldResponse
{
	/// <summary>
	/// Идентификатор локального поля
	/// </summary>
	public required string Id { get; init; }

	/// <summary>
	/// Идентификатор категории поля
	/// <see href="https://tracker.yandex.ru/admin/fields" />
	/// </summary>
	public required string CategoryId { get; init; }

	/// <summary>
	/// Название локального поля
	/// </summary>
	public required QueueLocalFieldName FieldName { get; init; }

	/// <summary>
	/// Тип локального поля
	/// </summary>
	public QueueLocalFieldType FieldType { get; init; }

	/// <summary>
	/// Объект с информацией об элементах списка
	/// </summary>
	public OptionsProviderInfo? OptionsProvider { get; init; }

	/// <summary>
	/// Порядковый номер в списке полей организации
	/// <see href="https://tracker.yandex.ru/admin/fields" />
	/// </summary>
	public int SerialNumberInFieldsOrganization { get; init; }

	/// <summary>
	/// Описание локального поля
	/// </summary>
	public string? Description { get; init; }

	/// <summary>
	/// Возможность редактировать значение поля:
	/// true - нельзя изменить;
	/// false - можно изменить
	/// </summary>
	public bool Readonly { get; init; }

	/// <summary>
	/// Признак отображения поля в интерфейсе:
	/// true - всегда отображать поле в интерфейсе
	/// false - не отображать поле в интерфейсе
	/// </summary>
	public bool Visible { get; init; }

	/// <summary>
	/// Признак видимости поля в интерфейсе:
	/// true — скрывать поле даже в том случае, если оно заполнено;
	/// false — не скрывать поле
	/// </summary>
	public bool Hidden { get; init; }

	/// <summary>
	/// Признак возможности указать в поле одновременно несколько значений (например, как в поле Теги):
	/// true — в поле можно указать несколько значений;
	/// false — в поле можно указать только одно значение.
	/// </summary>
	/// <remarks>
	/// Этот параметр допустимо использовать для полей следующих типов:
	/// StringFieldType — Текстовое однострочное поле;
	/// UserFieldType — Имя пользователя;
	/// выпадающий список.
	/// </remarks>
	public bool? Container { get; init; }
}

/// <summary>
/// Название локального поля очереди
/// </summary>
[DataContract]
public sealed record QueueLocalFieldName
{
	/// <summary>
	/// Название поля на английском языке
	/// </summary>
	[DataMember(Name = "en")]
	public required string En { get; set; }

	/// <summary>
	/// Название поля на русском языке
	/// </summary>
	[DataMember(Name = "ru")]
	public required string Ru { get; set; }
}

public sealed record OptionsProviderInfo
{
	/// <summary>
	/// Тип значений поля
	/// </summary>
	public required string Type { get; init; }

	/// <summary>
	/// Массив со значениями поля
	/// </summary>
	public Collection<JsonElement> Values { get; init; } = [];
}