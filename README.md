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
services.AddYandexTrackerClient();
```

Есть возможность использовать кеширование запросов к API Яндекс.Трекера для редко изменяемых, но часто используемых 
данных (список статусов, типов задач, пользователей и т.д.). Для включения кэширования необходимо при регистрации клиента
использовать метод `AddYandexTrackerClientCachingDecorator` и дополнительно зарегистрировать `YandexTrackerClientCachingOptions`.
В настройках кэширования можно задать TTl в минутах (2 минуты по умолчанию).

```csharp
services.AddOptions<YandexTrackerClientOptions>().Bind(configuration.GetSection("YandexTracker"));
services.AddOptions<YandexTrackerClientCachingOptions>().Bind(configuration.GetSection("YandexTracker"));
services.AddYandexTrackerClientCachingDecorator();
```