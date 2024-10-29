using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

/// <summary>
/// Поле задачи
/// </summary>
public sealed record IssueField
{
	/// <summary>
	/// Идентификатор поля
	/// </summary>
	public required string Id { get; init; }

	/// <summary>
	/// Ключ поля
	/// </summary>
	public required string Key { get; init; }

	/// <summary>
	/// Название поля
	/// </summary>
	public required string Name { get; init; }

	/// <summary>
	/// Описание поля
	/// </summary>
	public string? Description { get; init; }

	/// <summary>
	/// Возможность редактировать значение поля:
	/// true — значение поля нельзя изменить;
	/// false — значение поля можно изменить.
	/// </summary>
	public bool Readonly { get; init; }

	/// <summary>
	/// Ограничение списка значений:
	/// true — список значений не ограничен, можно задать любое значение;
	/// false — список значений ограничен настройками организации.
	/// </summary>
	public bool Options { get; init; }

	/// <summary>
	/// Наличие подсказки при вводе значения поля:
	/// true — при вводе значения появляется поисковая подсказка;
	/// false — функция поисковой подсказки отключена.
	/// </summary>
	public bool Suggest { get; init; }

	/// <summary>
	/// Информация о допустимых значениях поля
	/// </summary>
	public OptionsProviderInfo? OptionsProvider { get; init; }

	/// <summary>
	/// Информация о классе языка запроса.
	/// </summary>
	/// <remarks>Класс невозможно изменить с помощью API</remarks>
	public string? QueryProvider { get; init; }

	/// <summary>
	/// Информация о классе поисковой подсказки.
	/// </summary>
	/// <remarks>Класс подсказки невозможно изменить с помощью API.</remarks>
	public string? SuggestProvider { get; init; }

	/// <summary>
	/// Порядковый номер в списке полей организации:
	/// <see href="https://tracker.yandex.ru/admin/fields">https://tracker.yandex.ru/admin/fields</see>
	/// </summary>
	public int Order { get; init; }

	/// <summary>
	/// Идентификатор категории
	/// </summary>
	public required string CategoryId { get; init; }

	/// <summary>
	/// Информация о типе данных значения поля
	/// </summary>
	public required string Schema { get; init; }
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
	public Collection<object> Values { get; init; } = [];
}
