# Mindbox.YandexTracker


`Mindbox.YandexTracker` — это библиотека для взаимодействия с API Яндекс.Трекера.

**Ответственный трайб:** Framework

## Зачем использовать

В библиотеке поддержан необходимый для Mindbox минимум методов API:
- Получение, создание и удаление задач.
- Получение, создание и удаление проектов.
- Получение, создание и удаление очередей.
- Получение, создание и удаление комментариев к задачи.
- Получение и создание компонентов очереди.
- Получение, создание и удаление вложений.
- Получение пользователей, тегов, резолюций, типов и статусов задач, а также списка доступных полей очереди.

## Как использовать

Для работы с библиотекой необходимо зарегистрировать `YandexTrackerClient` и `YandexTrackerClientOptions` в вашем IoC-контейнере, используя сборку `Mindbox.YandexTracker.Template`.

```csharp
services.AddOptions<YandexTrackerClientOptions>().Bind(configuration.GetSection("YandexTracker"));
services.AddYandexTrackerClient(enableCaching: false);
```

Есть возможность использовать кеширование запросов к API Яндекс.Трекера для редко изменяемых, но часто используемых 
данных (список статусов, типов задач, пользователей и т.д.). Для включения кэширования необходимо при регистрации клиента
передать `enableCaching=true` и дополнительно зарегистрировать `YandexTrackerClientCachingOptions`.
В настройках кэширования можно задать TTl (2 минуты по умолчанию).

```csharp
services.AddOptions<YandexTrackerClientOptions>().Bind(configuration.GetSection("YandexTracker"));
services.AddOptions<YandexTrackerClientCachingOptions>().Bind(configuration.GetSection("YandexTracker"));
services.AddYandexTrackerClient(enableCaching: true);
```

Также рекомендуется использовать `Microsoft.Extensions.Http.Resilience` для настройки различных политик http запроса:

```csharp
services.AddOptions<YandexTrackerClientOptions>().Bind(configuration.GetSection("YandexTracker"));
services.AddOptions<YandexTrackerClientCachingOptions>().Bind(configuration.GetSection("YandexTracker"));
services.AddYandexTrackerClient(enableCaching: true)
        .AddResilienceHandler("ResilienceStrategy", builder =>
        {
            builder.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<HttpRequestException>()
                    .HandleResult(response =>
                    {
                        int statusCode = (int)response.StatusCode;
                        return statusCode >= 500 || statusCode == 429;
                    })
            });

            builder.AddTimeout(TimeSpan.FromSeconds(5));
        });
```

## Запуск тестов
Большинство тестов интеграционные.
Для их локального запуска нужно переименовать файл `example.appsettings.secret.json` в `appsettings.secret.json` и заполнить его поля.
В рамках тестов будет создана отдельная очередь, которая после прогона будет удалена.