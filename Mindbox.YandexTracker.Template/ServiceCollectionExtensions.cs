using Microsoft.Extensions.DependencyInjection;

namespace Mindbox.YandexTracker.Template;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddYandexTrackerClient(
		this IServiceCollection services,
		YandexTrackerClientOptions configureOptions)
	{
		return
			services
				.AddHttpClient()
				.AddMemoryCache()
				.Configure<YandexTrackerClientOptions>(option =>
				{
					option.Token = configureOptions.Token;
					option.Organization = configureOptions.Organization;
				})
				.AddScoped<YandexTrackerClient>()
				.AddScoped<IYandexTrackerClient, CachingYandexTrackerClient>();
	}
}
