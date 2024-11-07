using System;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Mindbox.YandexTracker.Template;

public static class ServiceCollectionExtensions
{
	public static IHttpClientBuilder AddYandexTrackerClient(
		this IServiceCollection services,
		bool enableCaching,
		Action<IServiceProvider, HttpClient>? configureClient)
	{
		if (enableCaching)
		{
			return services
				.AddMemoryCache()
				.AddScoped<IYandexTrackerClient, YandexTrackerClientCachingDecorator>(sp =>
					new YandexTrackerClientCachingDecorator(
						sp.GetRequiredService<YandexTrackerClient>(),
						sp.GetRequiredService<IMemoryCache>(),
						sp.GetRequiredService<IOptionsMonitor<YandexTrackerClientCachingOptions>>()))
				.AddHttpClient<YandexTrackerClient>((sp, client) => configureClient?.Invoke(sp, client));
		}
		else
		{
			return services.AddHttpClient<YandexTrackerClient>((sp, client) => configureClient?.Invoke(sp, client));
		}
	}
}