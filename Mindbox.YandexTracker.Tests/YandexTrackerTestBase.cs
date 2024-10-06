using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mindbox.YandexTracker.Tests;

public abstract class YandexTrackerTestBase
{
	protected string TestQueueKey { get; } = "LIBRARYTESTS";
	protected IServiceProvider ServiceProvider { get; private set; } = null!;
	protected IYandexTrackerClient YandexTrackerClient { get; private set; } = null!;

	[TestInitialize]
	public async Task TestInitializeAsync()
	{
		var serviceCollection = new ServiceCollection();

		SetupServices(serviceCollection);

		ServiceProvider = serviceCollection.BuildServiceProvider();
		YandexTrackerClient = GetRequiredService<IYandexTrackerClient>();
		await CreateQueueAsync();
	}

	[TestCleanup]
	public async Task TestCleanupAsync()
	{
		await YandexTrackerClient.DeleteQueueAsync(TestQueueKey);
	}

	private async Task CreateQueueAsync()
	{
		await YandexTrackerClient.CreateQueueAsync(new CreateQueueRequest
		{
			Key = TestQueueKey,
			DefaultType = "task",
			LeadKey = "ya.toporow",
			Name = "TestQueueKey",
			IssutTypesConfig =
			[
				new()
				{
					IssueType = "task",
					Workflow = "developmentPresetWorkflow",
					Resolutions =
					[
						"wontFix"
					]
				}
			]
		});
	}

	private static void SetupServices(IServiceCollection services)
	{
		var configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.tests.json")
			.Build();

		services
			.AddHttpClient()
			.Configure<YandexTrackerClientOptions>(configuration.GetSection("YandexTrackerOptions"))
			.AddScoped<IYandexTrackerClient, YandexTrackerClient>();
	}

	protected T GetRequiredService<T>() where T : notnull
		=> ServiceProvider.GetRequiredService<T>();
}