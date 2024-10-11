using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mindbox.YandexTracker;

public interface IYandexTrackerClient : IDisposable
{
	Task<Queue> GetQueueAsync(
		string queueKey,
		QueueExpandData? expand = null,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Queue>> GetQueuesAsync(
		QueuesExpandData? expand = null,
		CancellationToken cancellationToken = default);

	Task<Queue> CreateQueueAsync(
		Queue queue,
		CancellationToken cancellationToken = default);

	Task DeleteQueueAsync(string queueKey, CancellationToken cancellationToken = default);

	Task<Issue> GetIssueAsync(
		string issueKey,
		IssueExpandData? expand = null,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Issue>> GetIssuesFromQueueAsync(
		string queueKey,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Issue>> GetIssuesFromByAsync(
		IReadOnlyList<string> keys,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Issue>> GetIssuesByFilterAsync(
		IssuesFilter issuesFilter,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Issue>> GetIssuesByQueryAsync(
		string query,
		IssuesExpandData? expand = null,
		CancellationToken cancellationToken = default);

	Task<Issue> CreateIssueAsync(Issue issue, CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Component>> GetComponentsAsync(CancellationToken cancellationToken = default);

	Task<Component> CreateComponentAsync(
		string componentName,
		string queueKey,
		string? description = null,
		string? leadLogin = null,
		bool? assignAuto = null,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Comment>> GetCommentsAsync(
		string issueKey,
		CommentExpandData? expand = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Создает комментарий
	/// </summary>
	/// <param name="issueKey">Ключ задачи</param>
	/// <param name="comment">Комментарий</param>
	/// <param name="addAuthorToFollowers">Добавить автора комментария в наблюдатели.
	/// Если параметр не задан, то применится дефолтное поведение Яндекс.Трекера (true)</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<Comment> CreateCommentAsync(
		string issueKey,
		Comment comment,
		bool? addAuthorToFollowers = null,
		CancellationToken cancellationToken = default);

	Task DeleteCommentAsync(
		string issueKey,
		int commentId,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Attachment>> GetAttachmentsAsync(string issueKey, CancellationToken cancellationToken = default);

	Task<Attachment> CreateAttachmentAsync(
		string issueKey,
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default);

	Task DeleteAttachmentAsync(
		string issueKey,
		string attachmentKey,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<string>> GetTagsAsync(string queueKey, CancellationToken cancellationToken = default);

	/// <summary>
	/// Создает проект
	/// </summary>
	/// <param name="entityType">Тип сущности (проект или портфель)</param>
	/// <param name="project">Проект</param>
	/// <param name="returnedFields">Дополнительные поля сущности, которые будут включены в ответ</param>
	/// <param name="cancellationToken"></param>
	/// <returns>Вернутся данные по проекту с проставленным значениями от Яндекс.Трекера</returns>
	Task<Project> CreateProjectAsync(
		ProjectEntityType entityType,
		Project project,
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
	Task DeleteProjectAsync(
		ProjectEntityType entityType,
		int projectShortId,
		bool? deleteWithBoard = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Получение проектов
	/// </summary>
	/// <param name="entityType"></param>
	/// <param name="project"></param>
	/// <param name="returnedFields">Дополнительные поля, которые будут включены в ответ.</param>
	/// <param name="input">Подстрока в названии сущности</param>
	/// <param name="orderBy">
	/// Параметры сортировки задач. В параметре можно указать ключ любого поля,
	/// по которому будет производиться сортировка.
	/// </param>
	/// <param name="orderAscending">Направление сортировки</param>
	/// <param name="rootOnly">Выводить только не вложенные сущности.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>Вернутся данные по проектам с проставленными значениями от Яндекс.Трекера</returns>
	Task<IReadOnlyList<Project>> GetProjectsAsync(
		ProjectEntityType entityType,
		Project project,
		ProjectFieldData? returnedFields = null,
		string? input = null,
		string? orderBy = null,
		bool? orderAscending = null,
		bool? rootOnly = null,
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<IssueField>> GetAccessibleFieldsForIssueAsync(
		string queueKey,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Возвращает информацию о пользователе, от имени которого выполняются запросы.
	/// </summary>
	Task<UserDetailedInfo> GetMyselfAsync(CancellationToken cancellationToken = default);

	Task<IReadOnlyList<UserDetailedInfo>> GetUsersAsync(CancellationToken cancellationToken = default);

	Task<UserDetailedInfo> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);

	Task<IReadOnlyList<IssueType>> GetIssueTypesAsync(
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Resolution>> GetResolutionsAsync(
		CancellationToken cancellationToken = default);

	Task<IReadOnlyList<IssueStatus>> GetIssueStatusesAsync(
		CancellationToken cancellationToken = default);
}
