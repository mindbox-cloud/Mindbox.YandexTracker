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

namespace Mindbox.YandexTracker.Template;

/// <summary>
/// Настройки кэширования данных клиента Яндекс.Трекера.
/// </summary>
public sealed record YandexTrackerClientCachingOptions
{
	/// <summary>
	/// Префикс для ключа кэширования.
	/// </summary>
	/// <remarks>
	/// Можно переопределить, если в рамках одного приложения нужно использовать несколько клиентов для разных пользователей.
	/// </remarks>
	public string CacheKeyPrefix { get; set; } = "MindboxYandexTrackerClientCache";
	/// <summary>
	/// Время жизни данных в кэше.
	/// </summary>
	public TimeSpan Ttl { get; set; } = TimeSpan.FromMinutes(2);
}