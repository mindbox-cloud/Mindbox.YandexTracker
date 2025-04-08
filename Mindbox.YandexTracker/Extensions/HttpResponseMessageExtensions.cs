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

using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Mindbox.YandexTracker;

internal static class HttpResponseMessageExtensions
{
	/// <summary>
	/// Десериализует HttpResponseMessage Content в результирующий тип.
	/// </summary>
	/// <param name="httpResponse"> <see cref="HttpResponseMessage"/> с API YandexTracker. </param>
	/// <param name="cancellationToken"> Токен отмены задачи. </param>
	/// <typeparam name="TResult"> Тип, в который необходимо десериализовать из HttpResponseMessage Content. </typeparam>
	/// <returns> Десериализованное значение </returns>
	/// <exception cref="YandexTrackerException" >Исключение выбрасывается,
	/// когда пришёл невалидный JSON с YandexTracker API. </exception>
	internal static async Task<TResult?> DeserializeYandexTrackerResponseContentAsync<TResult>(
		this HttpResponseMessage httpResponse,
		CancellationToken cancellationToken = default)
	{
		TResult result;
		var resultContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

		try
		{
			result = JsonSerializer
				.Deserialize<TResult>(resultContent, YandexTrackerConstants.YandexTrackerJsonSerializerOptions)!;
		}
		catch (JsonException jsonException)
		{
			throw new YandexTrackerException(
				$"Invalid JSON was provided by YandexTracker API: {resultContent}",
				HttpStatusCode.InternalServerError, jsonException);
		}

		return result;
	}
}