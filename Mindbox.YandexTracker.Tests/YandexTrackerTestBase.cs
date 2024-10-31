using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mindbox.YandexTracker.Tests;

public abstract class YandexTrackerTestBase
{
	protected static string TestQueueKey { get; private set; } = null!;
	protected static string CurrentUserId { get; private set; } = null!;
	protected static string CurrentUserLogin { get; private set; } = null!;
	protected static IServiceProvider ServiceProvider { get; private set; } = null!;
	protected static IYandexTrackerClient YandexTrackerClient { get; private set; } = null!;

	[ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
	public static async Task TestInitializeAsync(TestContext context)
	{
		var serviceCollection = new ServiceCollection();

		var configuration = new ConfigurationBuilder()
			// основной файл
			.AddJsonFile("appsettings.json")
			// секреты, которые в .gitignore, чтобы случайно не залить их в репозиторий
			.AddJsonFile("appsettings.secret.json", true)
			// для передачи параметров из CI/CD в GitHub Actions через env variables
			.AddEnvironmentVariables()
			.Build();

		SetupServices(serviceCollection, configuration);

		ServiceProvider = serviceCollection.BuildServiceProvider();
		YandexTrackerClient = GetRequiredService<IYandexTrackerClient>();

		// узнаем инфу о пользователи, от имени которого выполняем запрос
		var userInfo = await YandexTrackerClient.GetMyselfAsync();
		CurrentUserId = userInfo.Id.ToString(CultureInfo.InvariantCulture);
		CurrentUserLogin = userInfo.Login;

		// в ключе может быть до 15 латинских символов
		TestQueueKey = $"TEST{StringHelper.GetUniqueString(length: 11)}";
		await CreateQueueAsync();
	}

	[ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass)]
	public static async Task TestCleanupAsync()
	{
		await YandexTrackerClient.DeleteQueueAsync(TestQueueKey);
	}

	private static async Task CreateQueueAsync()
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
			Lead = new UserShortInfo { Id = CurrentUserId, Display = CurrentUserLogin },
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

	protected static T GetRequiredService<T>() where T : notnull
		=> ServiceProvider.GetRequiredService<T>();
}