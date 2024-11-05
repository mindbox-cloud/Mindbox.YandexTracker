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
	Task<IReadOnlyList<GetQueuesResponse>> GetQueuesAsync(
		QueuesExpandData? expand = null,
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

	// TODO У всех серчи апи нет параметра order.
	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/search-issues"/>
	/// </remarks>
	Task<IReadOnlyList<GetIssueResponse>> GetIssuesFromQueueAsync(
		string queueKey,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/search-issues"/>
	/// </remarks>
	Task<IReadOnlyList<GetIssueResponse>> GetIssuesByKeysAsync(
		IReadOnlyList<string> issueKeys,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/search-issues"/>
	/// </remarks>
	Task<IReadOnlyList<GetIssueResponse>> GetIssuesByFilterAsync(
		GetIssuesByFilterRequest request,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/search-issues"/>
	/// </remarks>
	Task<IReadOnlyList<GetIssueResponse>> GetIssuesByQueryAsync(
		string query,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/create-issue"/>
	/// </remarks>
	Task<CreateIssueResponse> CreateIssueAsync(CreateIssueRequest request, CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/get-components"/>
	/// </remarks>
	Task<IReadOnlyList<GetComponentResponse>> GetComponentsAsync(CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/post-component"/>
	/// </remarks>
	Task<CreateComponentResponse> CreateComponentAsync(
		CreateComponentRequest request,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/get-comments"/>
	/// </remarks>
	Task<IReadOnlyList<GetCommentsResponse>> GetCommentsAsync(
		string issueKey,
		CommentExpandData? expand = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Создает комментарий
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
	Task<IReadOnlyList<GetAttachmentResponse>> GetAttachmentsAsync(
		string issueKey,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/post-attachment"/>
	/// </remarks>
	Task<CreateAttachmentResponse> CreateAttachmentAsync(
		string issueKey,
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default);

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
	Task<IReadOnlyList<string>> GetTagsAsync(string queueKey, CancellationToken cancellationToken = default);

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
	/// <param name="cancellationToken"></param>
	/// <returns>Вернутся данные по проектам с проставленными значениями от Яндекс.Трекера</returns>
	/// <remarks>
	/// <see href="https://yandex.cloud/ru/docs/tracker/concepts/entities/search-entities"/>
	/// </remarks>
	Task<IReadOnlyList<ProjectInfo>> GetProjectsAsync(
		ProjectEntityType entityType,
		GetProjectsRequest request,
		ProjectFieldData? returnedFields = null,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/queues/get-local-fields"/>
	/// </remarks>
	Task<IReadOnlyList<GetIssueFieldsResponse>> GetLocalQueueFieldsAsync(
		string queueKey,
		CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/concepts/issues/get-global-fields"/>
	/// </remarks>
	Task<IReadOnlyList<GetIssueFieldsResponse>> GetGlobalFieldsAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Возвращает информацию о пользователе, от имени которого выполняются запросы.
	/// </summary>
	Task<UserDetailedInfoDto> GetMyselfAsync(CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.ru/support/tracker/ru/get-users"/>
	/// </remarks>
	Task<IReadOnlyList<UserDetailedInfoDto>> GetUsersAsync(CancellationToken cancellationToken = default);

	/// <remarks>
	/// <see href="https://yandex.cloud/ru/docs/tracker/get-user-info"/>
	/// </remarks>
	Task<UserDetailedInfoDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);

	Task<IReadOnlyList<GetIssueTypeResponse>> GetIssueTypesAsync(
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<GetResolutionResponse>> GetResolutionsAsync(
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<GetIssueStatusResponse>> GetIssueStatusesAsync(
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
	/// Возможные категории для поля
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<IReadOnlyList<GetFieldCategoriesResponse>> GetFieldCategoriesAsync(CancellationToken cancellationToken = default);
}
