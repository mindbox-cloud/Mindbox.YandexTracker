using Microsoft.Extensions.DependencyInjection;

namespace Mindbox.YandexTracker.Template;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddYandexTrackerClient(
		this IServiceCollection services,
		YandexTrackerClientOptions configureOptions,
		YandexTrackerClientCachingDecoratorOptions decoratorOptions,
		bool enableRarelyChaningDataCaching)
	{
		services = services
			.AddHttpClient()
			.AddMemoryCache()
			.Configure<YandexTrackerClientOptions>(option =>
			{
				option.OAuthToken = configureOptions.OAuthToken;
				option.Organization = configureOptions.Organization;
			})
			.Configure<YandexTrackerClientCachingDecoratorOptions>(option =>
			{
				option.TTLInMinutes = decoratorOptions.TTLInMinutes;
				option.CacheKeyPrefix = decoratorOptions.CacheKeyPrefix;
			});

		if (enableRarelyChaningDataCaching)
		{
			services = services
				.AddScoped<YandexTrackerClient>()
				.AddScoped<IYandexTrackerClient, YandexTrackerClientCachingDecorator>();
		}
		else
		{
			services = services.AddScoped<IYandexTrackerClient, YandexTrackerClient>();
		}

		return services;
	}
}
