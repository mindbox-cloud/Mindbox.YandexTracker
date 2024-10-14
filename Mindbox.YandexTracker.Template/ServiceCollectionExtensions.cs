using Microsoft.Extensions.DependencyInjection;

namespace Mindbox.YandexTracker.Template;

public static class ServiceCollectionExtensions
{

	public static IServiceCollection AddYandexTrackerClient(
		this IServiceCollection services,
		YandexTrackerClientOptions configureOptions,
		YandexTrackerClientCachingOptions? cashingOptions = null)
	{
		services = services
			.AddHttpClient()
			.AddMemoryCache()
			.Configure<YandexTrackerClientOptions>(option =>
			{
				option.OAuthToken = configureOptions.OAuthToken;
				option.Organization = configureOptions.Organization;
			});

		if (cashingOptions is not null)
		{
			services = services
				.Configure<YandexTrackerClientCachingOptions>(option =>
				{
					option.Ttl = cashingOptions.Ttl;
					option.CacheKeyPrefix = cashingOptions.CacheKeyPrefix;
				})
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
