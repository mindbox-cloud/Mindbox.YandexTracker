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

Для работы с библиотекой необходимо зарегистрировать `YandexTrackerClient` в вашем IoC-контейнере, используя сборку `Mindbox.YandexTracker.Template`.

```csharp
services.AddYandexTrackerClient(new YandexTrackerClientOptions
{
    Organization = "your_organization_id",
    OAuthToken = "your_private_token";
});
```

Есть возможность использовать кеширование запросов к API Яндекс.Трекера для редко изменяемых, но часто используемых 
данных (список статусов, типов задач, пользователей и т.д.). Для включения кэширования необходимо при регистрации клиента
в IoC указать настройки кэширования. В настройках кэширования можно задать TTl в минутах.

```csharp
services.AddYandexTrackerClient(new YandexTrackerClientOptions
{
    Organization = "your_organization_id",
    OAuthToken = "your_private_token";
}, 
new YandexTrackerClientCachingDecoratorOptions
{
    TTL = your_ttl // 2 минуты по умолчанию
});
```