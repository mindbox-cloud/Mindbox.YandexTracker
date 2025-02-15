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
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mindbox.YandexTracker;

/// <summary>
/// Клиент для взаимодействия с API Яндекс.Трекера.
/// </summary>
public interface IYandexTrackerClient : IDisposable
{
	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/queues/get-queue"/>
	/// </remarks>
	Task<GetQueuesResponse> GetQueueAsync(
		string queueKey,
		QueueExpandData? expand = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.cloud/ru/docs/tracker/concepts/queues/get-queues"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<GetQueuesResponse>> GetQueuesAsync(
		QueuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.cloud/ru/docs/tracker/concepts/queues/create-queue"/>
	/// </remarks>
	Task<CreateQueueResponse> CreateQueueAsync(
		CreateQueueRequest request,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.cloud/ru/docs/tracker/concepts/queues/delete-queue"/>
	/// </remarks>
	Task DeleteQueueAsync(string queueKey, CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.cloud/ru/docs/tracker/concepts/issues/get-issue"/>
	/// </remarks>
	Task<GetIssueResponse> GetIssueAsync(
		string issueKey,
		IssueExpandData? expand = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/search-issues"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<GetIssueResponse>> GetIssuesFromQueueAsync(
		string queueKey,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/search-issues"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<GetIssueResponse>> GetIssuesByKeysAsync(
		IReadOnlyList<string> issueKeys,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/search-issues"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<GetIssueResponse>> GetIssuesByFilterAsync(
		GetIssuesByFilterRequest request,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/search-issues"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<GetIssueResponse>> GetIssuesByQueryAsync(
		string query,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.cloud/ru/docs/tracker/concepts/issues/get-changelog"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<GetIssueChangelogResponse>> GetIssueChangelogAsync(
		string issueKey,
		string? fieldKey = null,
		IssueChangeType? type = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Создает новую задачу.
	/// </summary>
	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/create-issue"/>
	/// </remarks>
	Task<CreateIssueResponse> CreateIssueAsync(CreateIssueRequest request, CancellationToken cancellationToken = default);

	/// <summary>
	/// Обновляет существующую задачу.
	/// </summary>
	/// <param name="issueKey">Ключ задачи</param>
	/// <param name="request">Данные для обновления.</param>
	/// <param name="sendNotifications">Нужно ли отправлять уведомления об изменениях.</param>
	/// <param name="cancellationToken">Токен отмены.</param>
	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/patch-issue"/>
	/// </remarks>
	Task<UpdateIssueResponse> UpdateIssueAsync(
		string issueKey,
		UpdateIssueRequest request,
		bool sendNotifications = true,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Импортирует новую задачу в Трекер.
	/// </summary>
	/// <remarks>
	/// ВАЖНО: Должен вызываться только от администратора системы.
	/// В отличие от <see cref="CreateIssueAsync"/>, позволяет:
	/// - указать дату создания тикета и пользователя, который создал тикет;
	/// - последнюю дату изменения тикета и пользователя, который последний раз изменял тикет;
	/// - дату и время проставления резолюции и пользователя, который проставил резолюцию.
	///  <see href="https://yandex.cloud/ru/docs/tracker/concepts/import/import-ticket"/>
	/// </remarks>
	Task<ImportIssueResponse> ImportIssueAsync(ImportIssueRequest request, CancellationToken cancellationToken = default);

	/// <summary>
	/// Создает запрос на массовое редактирование задач.
	/// </summary>
	/// <remarks>
	/// Само редактирование выполняется в фоне.
	/// <see chref="https://yandex.cloud/ru/docs/tracker/concepts/bulkchange/bulk-update-issues"/>
	/// </remarks>
	Task<IssuesBulkUpdateResponse> BulkUpdateIssuesAsync(
		IssuesBulkUpdateRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Проверяет статус выполнения запроса на массовое редактирование задач.
	/// </summary>
	Task<BulkChangeStatusResponse> GetBulkChangeStatusAsync(
		string bulkChangeId,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/get-components"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<GetComponentResponse>> GetComponentsAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/post-component"/>
	/// </remarks>
	Task<CreateComponentResponse> CreateComponentAsync(
		CreateComponentRequest request,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/get-comments"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<GetCommentsResponse>> GetCommentsAsync(
		string issueKey,
		CommentExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Создает комментарий к задаче.
	/// </summary>
	/// <param name="issueKey">Ключ задачи</param>
	/// <param name="request">Запрос</param>
	/// <param name="addAuthorToFollowers">Добавить автора комментария в наблюдатели.
	/// Если параметр не задан, то применится дефолтное поведение Яндекс.Трекера (true)</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/add-comment"/>
	/// </remarks>
	Task<CreateCommentResponse> CreateCommentAsync(
		string issueKey,
		CreateCommentRequest request,
		bool? addAuthorToFollowers = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Импортирует комментарий в задачу.
	/// </summary>
	/// <param name="issueKey">Ключ задачи</param>
	/// <param name="request">Запрос</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/import/import-comments"/>
	/// </remarks>
	Task<ImportCommentResponse> ImportCommentAsync(
		string issueKey,
		ImportCommentRequest request,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/delete-comment"/>
	/// </remarks>
	Task DeleteCommentAsync(
		string issueKey,
		int commentId,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/get-attachments-list"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<GetAttachmentResponse>> GetAttachmentsAsync(
		string issueKey,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/post-attachment"/>
	/// </remarks>
	Task<CreateAttachmentResponse> CreateAttachmentAsync(
		string issueKey,
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Импортирует вложение в задачу.
	/// </summary>
	/// <remarks>
	/// В отличие от <see cref="CreateAttachmentAsync"/> или <see cref="CreateTemporaryAttachmentAsync"/>,
	/// позволяет указать дату создания вложения и пользователя, который создал вложение.
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/import/import-attachments"/>
	/// </remarks>
	Task<ImportAttachmentResponse> ImportAttachmentToIssueAsync(
		string issueKey,
		Stream fileStream,
		string newFileName,
		DateTime createdAt,
		string createdBy,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Импортирует вложение в комментарий задачи.
	/// </summary>
	/// <remarks>
	/// В отличие от <see cref="CreateAttachmentAsync"/> или <see cref="CreateTemporaryAttachmentAsync"/>,
	/// позволяет указать дату создания вложения и пользователя, который создал вложение.
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/import/import-attachments"/>
	/// </remarks>
	Task<ImportAttachmentResponse> ImportAttachmentToIssueCommentAsync(
		string issueKey,
		string commentId,
		Stream fileStream,
		string newFileName,
		DateTime createdAt,
		string createdBy,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Создает временное вложение, которое потом должно быть прикреплено к задаче, комментарию или сущности (проект/портфель).
	/// </summary>
	/// <remarks>
	/// <see href="https://yandex.cloud/ru/docs/tracker/concepts/issues/temp-attachment"/>
	/// </remarks>
	Task<CreateAttachmentResponse> CreateTemporaryAttachmentAsync(
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/delete-attachment"/>
	/// </remarks>
	Task DeleteAttachmentAsync(
		string issueKey,
		string attachmentKey,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/queues/get-tags"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<string>> GetTagsAsync(
		string queueKey,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Создает проект
	/// </summary>
	/// <param name="entityType">Тип сущности (проект или портфель)</param>
	/// <param name="request">Запрос</param>
	/// <param name="returnedFields">Дополнительные поля сущности, которые будут включены в ответ</param>
	/// <param name="cancellationToken"></param>
	/// <returns>Вернутся данные по проекту с проставленным значениями от Яндекс.Трекера</returns>
	/// <remarks>
	/// <see href="https://yandex.cloud/ru/docs/tracker/concepts/entities/create-entity"/>
	/// </remarks>
	Task<CreateProjectResponse> CreateProjectAsync(
		ProjectEntityType entityType,
		CreateProjectRequest request,
		ProjectFieldData? returnedFields = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Удаляет проект
	/// </summary>
	/// <param name="entityType">Тип сущности (проект или портфель)</param>
	/// <param name="projectShortId">Идентификатор проекта</param>
	/// <param name="deleteWithBoard">Удалить доску, связанную с проектом</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <remarks>
	/// <see href="https://yandex.cloud/ru/docs/tracker/concepts/entities/delete-entity"/>
	/// </remarks>
	Task DeleteProjectAsync(
		ProjectEntityType entityType,
		int projectShortId,
		bool? deleteWithBoard = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Получение проектов
	/// </summary>
	/// <param name="entityType"></param>
	/// <param name="request"></param>
	/// <param name="returnedFields">Дополнительные поля, которые будут включены в ответ.</param>
	/// <param name="paginationSettings">Настройки постраничного отображения результатов</param>
	/// <param name="cancellationToken"></param>
	/// <returns>Вернутся данные по проектам с проставленными значениями от Яндекс.Трекера</returns>
	/// <remarks>
	/// <see href="https://yandex.cloud/ru/docs/tracker/concepts/entities/search-entities"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<ProjectInfo>> GetProjectsAsync(
		ProjectEntityType entityType,
		GetProjectsRequest request,
		ProjectFieldData? returnedFields = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/queues/get-local-fields"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<GetIssueFieldsResponse>> GetLocalQueueFieldsAsync(
		string queueKey,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/get-global-fields"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<GetIssueFieldsResponse>> GetGlobalFieldsAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Возвращает информацию о пользователе, от имени которого выполняются запросы.
	/// </summary>
	Task<UserDetailedInfoDto> GetMyselfAsync(CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/get-users"/>
	/// </remarks>
	Task<YandexTrackerCollectionResponse<UserDetailedInfoDto>> GetUsersAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.cloud/ru/docs/tracker/get-user-info"/>
	/// </remarks>
	Task<UserDetailedInfoDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);

	Task<YandexTrackerCollectionResponse<GetIssueTypeResponse>> GetIssueTypesAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	Task<YandexTrackerCollectionResponse<GetResolutionResponse>> GetResolutionsAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	Task<YandexTrackerCollectionResponse<GetIssueStatusResponse>> GetIssueStatusesAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// <see href="https://yandex.cloud/ru/docs/tracker/concepts/queues/create-local-field" />
	/// </summary>
	/// <param name="queueKey">Ключ очереди</param>
	/// <param name="request">Запрос</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<CreateQueueLocalFieldResponse> CreateLocalFieldInQueueAsync(
		string queueKey,
		CreateQueueLocalFieldRequest request,
		CancellationToken cancellationToken = default);


	/// <summary>
	/// <see href="https://yandex.cloud/ru/docs/tracker/concepts/queues/update-local-field" />
	/// </summary>
	/// <param name="queueKey">Ключ очереди</param>
	/// <param name="fieldKey">Ключ поля</param>
	/// <param name="request">Запрос</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<UpdateQueueLocalFieldResponse> UpdateLocalFieldInQueueAsync(
		string queueKey,
		string fieldKey,
		UpdateQueueLocalFieldRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Возможные категории для поля
	/// </summary>
	/// <param name="paginationSettings">Настройки постраничного отображения результатов</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<YandexTrackerCollectionResponse<GetFieldCategoriesResponse>> GetFieldCategoriesAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default);
}
