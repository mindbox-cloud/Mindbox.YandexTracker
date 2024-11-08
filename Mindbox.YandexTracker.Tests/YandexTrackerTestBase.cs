// Copyright 2024 Mindbox Ltd
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
		CurrentUserId = userInfo.Uid.ToString(CultureInfo.InvariantCulture);
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
		await YandexTrackerClient.CreateQueueAsync(new CreateQueueRequest
		{
			Key = TestQueueKey,
			DefaultType = "task",
			Lead = CurrentUserLogin,
			Name = TestQueueKey.ToUpperInvariant(),
			IssueTypesConfig =
			[
				new()
				{
					IssueType = "task",
					Workflow = "developmentPresetWorkflow",
					Resolutions = ["wontFix"]
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