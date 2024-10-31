using Microsoft.Extensions.DependencyInjection;

namespace Mindbox.YandexTracker.Template;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddYandexTrackerClient(this IServiceCollection services, bool enableCaching)
	{
		return services
			.AddHttpClient()
			.AddScoped<IYandexTrackerClient, YandexTrackerClient>();
	}
}