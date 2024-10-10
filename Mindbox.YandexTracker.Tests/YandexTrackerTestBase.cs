using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mindbox.YandexTracker.Tests;

public abstract class YandexTrackerTestBase
{
	protected string TestQueueKey { get; private set; } = null!;
	protected string CurrentUserId { get; private set; } = null!;
	protected string CurrentUserLogin { get; private set; } = null!;
	protected IServiceProvider ServiceProvider { get; private set; } = null!;
	protected IYandexTrackerClient YandexTrackerClient { get; private set; } = null!;

	[TestInitialize]
	public async Task TestInitializeAsync()
	{
		var serviceCollection = new ServiceCollection();

		var configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.tests.json")
			.Build();

		SetupServices(serviceCollection, configuration);

		ServiceProvider = serviceCollection.BuildServiceProvider();
		YandexTrackerClient = GetRequiredService<IYandexTrackerClient>();

		// в ключе может быть до 15 латинских символов
		TestQueueKey = $"TEST{StringHelper.GetUniqueString(length: 11)}";
		CurrentUserId = configuration.GetValue<string>("CurrentUserId")!;
		CurrentUserLogin = configuration.GetValue<string>("CurrentUserLogin")!;
		await CreateQueueAsync();
	}

	[TestCleanup]
	public async Task TestCleanupAsync()
	{
		await YandexTrackerClient.DeleteQueueAsync(TestQueueKey);
	}

	private async Task CreateQueueAsync()
	{
		await YandexTrackerClient.CreateQueueAsync(new Queue
		{
			Key = TestQueueKey,
			DefaultType = new IssueType
			{
				Id = 2,
				Key = "task",
				Name = "Задача"
			},
			Lead = new UserShortInfo { Id = CurrentUserId },
			Name = TestQueueKey.ToUpperInvariant(),
			IssueTypesConfig =
			[
				new()
				{
					IssueType = new IssueType
					{
						Id = 2,
						Key = "task",
						Name = "Задача"
					},
					Workflow = "developmentPresetWorkflow",
					Resolutions =
					[
						new()
						{
							Id = 2,
							Key = "wontFix",
							Name = "Не будет исправлено"
						}
					]
				}
			]
		});
	}

	private static void SetupServices(IServiceCollection services, IConfigurationRoot configuration)
	{
		services
			.AddHttpClient()
			.Configure<YandexTrackerClientOptions>(configuration.GetSection("YandexTrackerOptions"))
			.AddScoped<IYandexTrackerClient, YandexTrackerClient>();
	}

	protected T GetRequiredService<T>() where T : notnull
		=> ServiceProvider.GetRequiredService<T>();
}