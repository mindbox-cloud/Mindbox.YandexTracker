using Microsoft.Extensions.DependencyInjection;

namespace Mindbox.YandexTracker.Template;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddYandexTrackerClient(
		this IServiceCollection services,
		YandexTrackerClientOptions configureOptions,
		bool enableRarelyChaningDataCaching)
	{
		services = services
			.AddHttpClient()
			.AddMemoryCache()
			.Configure<YandexTrackerClientOptions>(option =>
			{
				option.Token = configureOptions.Token;
				option.Organization = configureOptions.Organization;
			});

		if (enableRarelyChaningDataCaching)
		{
			services = services
				.AddScoped<YandexTrackerClient>()
				.AddScoped<IYandexTrackerClient, CachingYandexTrackerClient>();
		}
		else
		{
			services = services.AddScoped<IYandexTrackerClient, YandexTrackerClient>();
		}

		return services;
	}
}
