using System;

namespace Mindbox.YandexTracker.Template;

public sealed record YandexTrackerClientCachingDecoratorOptions
{
	public string CacheKeyPrefix { get; set; } = "MindboxYandexTrackerClientCache";
	public TimeSpan TTLInMinutes { get; set; } = TimeSpan.FromMinutes(2);
}
