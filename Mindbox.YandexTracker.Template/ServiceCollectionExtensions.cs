using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Mindbox.YandexTracker.Template;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddYandexTrackerClient(this IServiceCollection services)
	{
		return services
			.AddHttpClient()
			.AddScoped<IYandexTrackerClient, YandexTrackerClient>();
	}

	public static IServiceCollection AddYandexTrackerClientCachingDecorator(this IServiceCollection services)
	{
		return services
			.AddHttpClient()
			.AddMemoryCache()
			.AddScoped<YandexTrackerClient>()
			.AddScoped<IYandexTrackerClient, YandexTrackerClientCachingDecorator>(sp => new YandexTrackerClientCachingDecorator(
				sp.GetRequiredService<YandexTrackerClient>(),
				sp.GetRequiredService<IMemoryCache>(),
				sp.GetRequiredService<IOptionsMonitor<YandexTrackerClientCachingOptions>>()));
	}
}