# Mindbox.YandexTracker

**Ответственный трайб:** Framework

## Описание

`Mindbox.YandexTracker` — это библиотека для взаимодействия с Яндекс Трекером, предназначенная для работы Issue'сами. Библиотека позволяет получать, создавать и удалять информацию о Issues, Projects, Queues, Comments,
Tags, Components, Users необходимую для переноса аналогичных сущностей с Github'а на Яндекс Трекер.

## Зачем использовать

Эта библиотека предоставляет интерфейс для работы с Issues из Github, включая:
- Получение, создание и удаление задач.
- Получение, создание и удаление проектов.
- Получение, создание и удаление очередей.
- Получение, создание и удаление комментариев к задачи.
- Получение и создание компонентов очереди.
- Получение, создание и удаление вложений.
- Получение пользователей, тегов, резолюций, типов и статусов задач, а также списка доступных полей очереди.
- Возможность кэширования следующих сущностей: пользователей, проектов, очередей, тегов, резолюций, возможных полей для очереди, компонентов, статусов задач и типов задач.

Данный пакет необходим для переноса задач с их содержимым из Github'а в Яндекс Трекер.

## Как использовать

Для работы с библиотекой необходимо зарегистрировать `YandexTrackerClient` в вашем IoC-контейнере, используя сборку `Mindbox.YandexTracker.Template`.

### Пример использования

Пример регистрации клиента в IoC-контейнере:

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
},
enableRarelyChaningDataCaching: false);
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
},
enableRarelyChaningDataCaching: true);```