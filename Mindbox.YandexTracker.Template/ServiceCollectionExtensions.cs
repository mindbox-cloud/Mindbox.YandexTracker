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
		Action<IServiceProvider, HttpClient>? configureClient = null)
	{
		if (enableCaching)
		{
			return services
				.AddMemoryCache()
				.AddTransient<IYandexTrackerClient, YandexTrackerClientCachingDecorator>(sp =>
					new YandexTrackerClientCachingDecorator(
						sp.GetRequiredService<YandexTrackerClient>(),
						sp.GetRequiredService<IMemoryCache>(),
						sp.GetRequiredService<IOptionsMonitor<YandexTrackerClientCachingOptions>>()))
				.AddHttpClient<YandexTrackerClient>((sp, client) => configureClient?.Invoke(sp, client));
		}
		else
		{
			return services.AddHttpClient<IYandexTrackerClient, YandexTrackerClient>(
				(sp, client) => configureClient?.Invoke(sp, client));
		}
	}
}