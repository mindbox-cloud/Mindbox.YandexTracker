using System;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Детальная информация о пользователе
/// </summary>
public sealed record UserDetailedInfo
{
	/// <summary>
	/// Уникальный идентификатор учетной записи пользователя в Tracker
	/// </summary>
	[JsonPropertyName( "uid")]
	public required string Id { get; init; }

	/// <summary>
	/// Логин пользователя
	/// </summary>
	[JsonPropertyName("login")]
	public required string Login { get; init; }

	/// <summary>
	/// Уникальный идентификатор аккаунта пользователя в Tracker
	/// </summary>
	[JsonPropertyName("trackerUid")]
	public required string TrackerUid { get; init; }

	/// <summary>
	/// Уникальный идентификатор аккаунта пользователя в организации Яндекс 360 для бизнеса и Яндекс ID
	/// </summary>
	[JsonPropertyName("passportUid")]
	public required string PassportUid { get; init; }

	/// <summary>
	/// Уникальный идентификатор пользователя в Yandex Cloud Organization
	/// </summary>
	[JsonPropertyName("cloudUid")]
	public required string CloudUid { get; init; }

	/// <summary>
	/// Имя пользователя
	/// </summary>
	[JsonPropertyName("firstName")]
	public required string FirstName { get; init; }

	/// <summary>
	/// Фамилия пользователя
	/// </summary>
	[JsonPropertyName("lastName")]
	public required string LastName { get; init; }

	/// <summary>
	/// Отображаемое имя пользователя
	/// </summary>
	[JsonPropertyName("display")]
	public required string Display { get; init; }

	/// <summary>
	/// Электронная почта пользователя
	/// </summary>
	[JsonPropertyName("email")]
	public required string Email { get; init; }

	/// <summary>
	/// Служебный параметр
	/// </summary>
	[JsonPropertyName("external")]
	public bool External { get; init; }

	/// <summary>
	/// Признак наличия у пользователя полного доступа к Tracker:
	/// true — полный доступ;
	/// false — только чтение
	/// </summary>
	[JsonPropertyName("hasLicense")]
	public bool HasLicense { get; init; }

	/// <summary>
	/// Статус пользователя в организации:
	/// true — пользователь удален из организации;
	/// false — действующий сотрудник организации
	/// </summary>
	[JsonPropertyName("dismissed")]
	public bool Dismissed { get; init; }

	/// <summary>
	/// Служебный параметр
	/// </summary>
	[JsonPropertyName("useNewFilters")]
	public bool UseNewFilters { get; init; }

	/// <summary>
	/// Признак принудительного отключения уведомлений для пользователя:
	/// true — уведомления отключены;
	/// false — уведомления включены
	/// </summary>
	[JsonPropertyName("disableNotifications")]
	public bool DisableNotifications { get; init; }

	/// <summary>
	/// Дата и время первой авторизации пользователя
	/// </summary>
	[JsonPropertyName("firstLoginDate")]
	public DateTime FirstLoginDateUtc { get; init; }

	/// <summary>
	/// Дата и время последней авторизации пользователя
	/// </summary>
	[JsonPropertyName("lastLoginDate")]
	public DateTime LastLoginDateUtc { get; init; }

	/// <summary>
	/// Способ добавления пользователя:
	/// true — с помощью приглашения на почту;
	/// false — другим способом.
	/// </summary>
	[JsonPropertyName("welcomeMailSent")]
	public bool WelcomeMailSent { get; init; }
}
