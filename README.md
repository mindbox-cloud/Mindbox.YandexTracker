# Mindbox.YandexTracker

**Ответственный трайб:** Framework

## Описание

`Mindbox.YandexTracker` — это библиотека для взаимодействия с API Яндекс.Трекера

## Зачем использовать

В библиотеке поддержан необходимый для Mindbox минимум методов API:
- Получение, создание и удаление задач.
- Получение, создание и удаление проектов.
- Получение, создание и удаление очередей.
- Получение, создание и удаление комментариев к задачи.
- Получение и создание компонентов очереди.
- Получение, создание и удаление вложений.
- Получение пользователей, тегов, резолюций, типов и статусов задач, а также списка доступных полей очереди.
- Возможность кэширования следующих сущностей: пользователей, проектов, очередей, тегов, резолюций, возможных полей для очереди, компонентов, статусов задач и типов задач.

## Как использовать

Для работы с библиотекой необходимо зарегистрировать `YandexTrackerClient` в вашем IoC-контейнере, используя сборку `Mindbox.YandexTracker.Template`.

### Пример использования

Пример регистрации клиента в IoC-контейнере:

```csharp
services.AddYandexTrackerClient(new YandexTrackerClientOptions
{
    Organization = "your_organization_id",
    OAuthToken = "your_private_token";
});
```

При регистрации клиента с кэшированием в IoC-контейнере:
```csharp
services.AddYandexTrackerClient(new YandexTrackerClientOptions
{
    Organization = "your_organization_id",
    OAuthToken = "your_private_token";
}, 
new YandexTrackerClientCachingDecoratorOptions
{
    CacheKeyPrefix = "cache_key_prefix", // "MindboxYandexTrackerClientCache" по умолчанию
    TTLInMinutes = your_ttl // 2 минуты по умолчанию
});```