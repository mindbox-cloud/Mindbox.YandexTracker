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
	/// когда пришёл невалидный JSON с YandexTracker API или к. </exception>
	internal static async Task<TResult?> DeserializeYandexTrackerResponseContentAsync<TResult>(
		this HttpResponseMessage httpResponse,
		CancellationToken cancellationToken = default)
	{
		TResult result;
		var resultContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
		var isValidJson = TryParseDocument(resultContent, out var jsonDocument, out var exception);

		if (!isValidJson)
			throw new YandexTrackerException(
				$"Invalid JSON was provided by YandexTracker API: {resultContent}",
				HttpStatusCode.BadGateway,
				exception);

		try
		{
			result = jsonDocument!
				.Deserialize<TResult>(YandexTrackerConstants.YandexTrackerJsonSerializerOptions)!;
		}
		catch (JsonException jsonException)
		{
			throw new YandexTrackerException(
				$"Invalid type was provided {typeof(TResult).Name} for deserialization JSON" +
				$" from YandexTracker API: {resultContent}",
				HttpStatusCode.InternalServerError, jsonException);
		}

		return result;
	}

	/// <summary>
	/// Метод для валидации и получения JsonDocument из строки.
	/// </summary>
	/// <param name="json"> Строка, которая представляет из себя JSON. </param>
	/// <param name="document"> Результат распаршивания строки. </param>
	/// <param name="exception"> <see cref="JsonException"/> для случая, когда распарсить JSON не удалось. </param>
	/// <returns> Результат попытки распарсить JSON строку. </returns>
	private static bool TryParseDocument(string json, out JsonDocument? document, out JsonException? exception)
	{
		try
		{
			document = JsonDocument.Parse(json);
			exception = null;
			return true;
		}
		catch (JsonException jsonException)
		{
			document = null;
			exception = jsonException;
			return false;
		}
	}
}