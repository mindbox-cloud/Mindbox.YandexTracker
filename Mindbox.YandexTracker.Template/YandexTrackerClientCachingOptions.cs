using System;

namespace Mindbox.YandexTracker.Template;

/// <summary>
/// Настройки кэширования данных клиента Яндекс.Трекера.
/// </summary>
public sealed record YandexTrackerClientCachingOptions
{
	/// <summary>
	/// Префикс для ключа кэширования.
	/// </summary>
	/// <remarks>
	/// Можно переопределить, если в рамках одного приложения нужно использовать несколько клиентов для разных пользователей.
	/// </remarks>
	public string CacheKeyPrefix { get; set; } = "MindboxYandexTrackerClientCache";
	/// <summary>
	/// Время жизни данных в кэше.
	/// </summary>
	public TimeSpan Ttl { get; set; } = TimeSpan.FromMinutes(2);
}
