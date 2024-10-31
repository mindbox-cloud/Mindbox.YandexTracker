using System;

namespace Mindbox.YandexTracker;

/// <summary>
/// Детальная информация о пользователе
/// </summary>
public sealed record UserDetailedInfo
{
	/// <summary>
	/// Уникальный идентификатор учетной записи пользователя в Tracker
	/// </summary>
	public required long Uid { get; init; }

	/// <summary>
	/// Логин пользователя
	/// </summary>
	public required string Login { get; init; }

	/// <summary>
	/// Уникальный идентификатор аккаунта пользователя в Tracker
	/// </summary>
	public required long TrackerUid { get; init; }

	/// <summary>
	/// Уникальный идентификатор аккаунта пользователя в организации Яндекс 360 для бизнеса и Яндекс ID
	/// </summary>
	/// <remarks>
	/// Заполнен, если пользователь добавлен через Яндекс 360 для бизнеса или Яндекс ID.
	/// </remarks>
	public long? PassportUid { get; init; }

	/// <summary>
	/// Уникальный идентификатор пользователя в Yandex Cloud Organization
	/// </summary>
	/// <remarks>
	/// Заполнен, если пользователь добавлен через Yandex Cloud Organization.
	/// </remarks>
	public string? CloudUid { get; init; }

	/// <summary>
	/// Имя пользователя
	/// </summary>
	public required string FirstName { get; init; }

	/// <summary>
	/// Фамилия пользователя
	/// </summary>
	public required string LastName { get; init; }

	/// <summary>
	/// Отображаемое имя пользователя
	/// </summary>
	public required string Display { get; init; }

	/// <summary>
	/// Электронная почта пользователя
	/// </summary>
	public required string Email { get; init; }

	/// <summary>
	/// Служебный параметр
	/// </summary>
	public bool External { get; init; }

	/// <summary>
	/// Признак наличия у пользователя полного доступа к Tracker:
	/// true — полный доступ;
	/// false — только чтение
	/// </summary>
	public bool HasLicense { get; init; }

	/// <summary>
	/// Статус пользователя в организации:
	/// true — пользователь удален из организации;
	/// false — действующий сотрудник организации
	/// </summary>
	public bool Dismissed { get; init; }

	/// <summary>
	/// Служебный параметр
	/// </summary>
	public bool UseNewFilters { get; init; }

	/// <summary>
	/// Признак принудительного отключения уведомлений для пользователя:
	/// true — уведомления отключены;
	/// false — уведомления включены
	/// </summary>
	public bool DisableNotifications { get; init; }

	/// <summary>
	/// Дата и время первой авторизации пользователя
	/// </summary>
	public DateTime? FirstLoginDateUtc { get; init; }

	/// <summary>
	/// Дата и время последней авторизации пользователя
	/// </summary>
	public DateTime? LastLoginDateUtc { get; init; }

	/// <summary>
	/// Способ добавления пользователя:
	/// true — с помощью приглашения на почту;
	/// false — другим способом.
	/// </summary>
	public bool WelcomeMailSent { get; init; }
}
