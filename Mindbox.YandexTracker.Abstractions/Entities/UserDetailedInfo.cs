using System;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Детальная информация о пользователе
/// </summary>
[DataContract]
public sealed record UserDetailedInfo
{
	/// <summary>
	/// Уникальный идентификатор учетной записи пользователя в Tracker
	/// </summary>
	[DataMember(Name = "uid")]
	public required string Id { get; init; }

	/// <summary>
	/// Логин пользователя
	/// </summary>
	[DataMember(Name = "login")]
	public required string Login { get; init; }

	/// <summary>
	/// Уникальный идентификатор аккаунта пользователя в Tracker
	/// </summary>
	[DataMember(Name = "trackerUid")]
	public required string TrackerUid { get; init; }

	/// <summary>
	/// Уникальный идентификатор аккаунта пользователя в организации Яндекс 360 для бизнеса и Яндекс ID
	/// </summary>
	[DataMember(Name = "passportUid")]
	public required string PassportUid { get; init; }

	/// <summary>
	/// Уникальный идентификатор пользователя в Yandex Cloud Organization
	/// </summary>
	[DataMember(Name = "cloudUid")]
	public required string CloudUid { get; init; }

	/// <summary>
	/// Имя пользователя
	/// </summary>
	[DataMember(Name = "firstName")]
	public required string FirstName { get; init; }

	/// <summary>
	/// Фамилия пользователя
	/// </summary>
	[DataMember(Name = "lastName")]
	public required string LastName { get; init; }

	/// <summary>
	/// Отображаемое имя пользователя
	/// </summary>
	[DataMember(Name = "display")]
	public required string Display { get; init; }

	/// <summary>
	/// Электронная почта пользователя
	/// </summary>
	[DataMember(Name = "email")]
	public required string Email { get; init; }

	/// <summary>
	/// Служебный параметр
	/// </summary>
	[DataMember(Name = "external")]
	public bool External { get; init; }

	/// <summary>
	/// Признак наличия у пользователя полного доступа к Tracker:
	/// true — полный доступ;
	/// false — только чтение
	/// </summary>
	[DataMember(Name = "hasLicense")]
	public bool HasLicense { get; init; }

	/// <summary>
	/// Статус пользователя в организации:
	/// true — пользователь удален из организации;
	/// false — действующий сотрудник организации
	/// </summary>
	[DataMember(Name = "dismissed")]
	public bool Dismissed { get; init; }

	/// <summary>
	/// Служебный параметр
	/// </summary>
	[DataMember(Name = "useNewFilters")]
	public bool UseNewFilters { get; init; }

	/// <summary>
	/// Признак принудительного отключения уведомлений для пользователя:
	/// true — уведомления отключены;
	/// false — уведомления включены
	/// </summary>
	[DataMember(Name = "disableNotifications")]
	public bool DisableNotifications { get; init; }

	/// <summary>
	/// Дата и время первой авторизации пользователя
	/// </summary>
	[DataMember(Name = "firstLoginDate")]
	public DateTime FirstLoginDateUtc { get; init; }

	/// <summary>
	/// Дата и время последней авторизации пользователя
	/// </summary>
	[DataMember(Name = "lastLoginDate")]
	public DateTime LastLoginDateUtc { get; init; }

	/// <summary>
	/// Способ добавления пользователя:
	/// true — с помощью приглашения на почту;
	/// false — другим способом.
	/// </summary>
	[DataMember(Name = "welcomeMailSent")]
	public bool WelcomeMailSent { get; init; }
}
