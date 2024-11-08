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

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Mindbox.YandexTracker.Template;


/// <inheritdoc />
public sealed class YandexTrackerClientCachingDecorator(
	IYandexTrackerClient yandexTrackerClient,
	IMemoryCache cache,
	IOptionsMonitor<YandexTrackerClientCachingOptions> options) : IYandexTrackerClient
{
	/// <inheritdoc />
	public Task<CreateAttachmentResponse> CreateAttachmentAsync(
		string issueKey,
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateAttachmentAsync(issueKey, fileStream, newFileName, cancellationToken);
	}

	/// <inheritdoc />
	public Task<CreateAttachmentResponse> CreateTemporaryAttachmentAsync(
		Stream fileStream,
		string? newFileName = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateTemporaryAttachmentAsync(fileStream, newFileName, cancellationToken);
	}

	/// <inheritdoc />
	public Task<CreateCommentResponse> CreateCommentAsync(
		string issueKey,
		CreateCommentRequest request,
		bool? addAuthorToFollowers = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateCommentAsync(issueKey, request, addAuthorToFollowers, cancellationToken);
	}

	/// <inheritdoc />
	public Task<CreateIssueResponse> CreateIssueAsync(CreateIssueRequest request, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateIssueAsync(request, cancellationToken);
	}

	/// <inheritdoc />
	public Task<CreateProjectResponse> CreateProjectAsync(
		ProjectEntityType entityType,
		CreateProjectRequest request,
		ProjectFieldData? returnedFields = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateProjectAsync(entityType, request, returnedFields, cancellationToken);
	}

	/// <inheritdoc />
	public Task<CreateQueueResponse> CreateQueueAsync(CreateQueueRequest request, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateQueueAsync(request, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteAttachmentAsync(string issueKey, string attachmentKey, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.DeleteAttachmentAsync(issueKey, attachmentKey, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteCommentAsync(string issueKey, int commentId, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.DeleteCommentAsync(issueKey, commentId, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteProjectAsync(
		ProjectEntityType entityType,
		int projectShortId,
		bool? deleteWithBoard = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.DeleteProjectAsync(entityType, projectShortId, deleteWithBoard, cancellationToken);
	}

	/// <inheritdoc />
	public Task DeleteQueueAsync(string queueKey, CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.DeleteQueueAsync(queueKey, cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueFieldsResponse>> GetLocalQueueFieldsAsync(
		string queueKey,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_localFields_{queueKey}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetLocalQueueFieldsAsync(queueKey, paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueFieldsResponse>> GetGlobalFieldsAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_globalFields";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetGlobalFieldsAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	/// <inheritdoc />
	public Task<UserDetailedInfoDto> GetMyselfAsync(CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetMyselfAsync(cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetAttachmentResponse>> GetAttachmentsAsync(
		string issueKey,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetAttachmentsAsync(issueKey, paginationSettings, cancellationToken);
	}

	/// <inheritdoc />
	public Task<CreateComponentResponse> CreateComponentAsync(
		CreateComponentRequest request,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateComponentAsync(
			request,
			cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetCommentsResponse>> GetCommentsAsync(
		string issueKey,
		CommentExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetCommentsAsync(issueKey, expand, paginationSettings, cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetComponentResponse>> GetComponentsAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_components";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetComponentsAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	/// <inheritdoc />
	public Task<GetIssueResponse> GetIssueAsync(
		string issueKey,
		IssueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetIssueAsync(issueKey, expand, cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueResponse>> GetIssuesByFilterAsync(
		GetIssuesByFilterRequest request,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetIssuesByFilterAsync(request, expand, paginationSettings, cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueResponse>> GetIssuesByQueryAsync(
		string query,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetIssuesByQueryAsync(query, expand, paginationSettings, cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueResponse>> GetIssuesByKeysAsync(
		IReadOnlyList<string> issueKeys,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetIssuesByKeysAsync(issueKeys, expand, paginationSettings, cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueResponse>> GetIssuesFromQueueAsync(
		string queueKey,
		IssuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.GetIssuesFromQueueAsync(queueKey, expand, paginationSettings, cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueStatusResponse>> GetIssueStatusesAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_issueStatuses";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetIssueStatusesAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	/// <inheritdoc />
	public Task<CreateQueueLocalFieldResponse> CreateLocalFieldInQueueAsync(
		string queueKey,
		CreateQueueLocalFieldRequest request,
		CancellationToken cancellationToken = default)
	{
		return yandexTrackerClient.CreateLocalFieldInQueueAsync(queueKey, request, cancellationToken);
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetFieldCategoriesResponse>> GetFieldCategoriesAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_localFieldCategories";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetFieldCategoriesAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetIssueTypeResponse>> GetIssueTypesAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_issueTypes";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetIssueTypesAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<ProjectInfo>> GetProjectsAsync(
		ProjectEntityType entityType,
		GetProjectsRequest request,
		ProjectFieldData? returnedFields = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_projects_{entityType}_{request.GetHashCode()}_{returnedFields}_" +
			$"{request.Input}_{request.OrderBy}_{request.OrderAsc}_{request.RootOnly}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetProjectsAsync(
					entityType,
					request,
					returnedFields,
					paginationSettings,
					cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	/// <inheritdoc />
	public Task<GetQueuesResponse> GetQueueAsync(
		string queueKey,
		QueueExpandData? expand = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_queues_{queueKey}_{expand}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetQueueAsync(queueKey, expand, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetQueuesResponse>> GetQueuesAsync(
		QueuesExpandData? expand = null,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_queues_{expand}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetQueuesAsync(expand, paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<GetResolutionResponse>> GetResolutionsAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_resolutions";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetResolutionsAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<string>> GetTagsAsync(
		string queueKey,
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_tags_{queueKey}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetTagsAsync(queueKey, paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	/// <inheritdoc />
	public Task<UserDetailedInfoDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_users_{userId}";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetUserByIdAsync(userId, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	/// <inheritdoc />
	public Task<YandexTrackerCollectionResponse<UserDetailedInfoDto>> GetUsersAsync(
		PaginationSettings? paginationSettings = null,
		CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{options.CurrentValue.CacheKeyPrefix}_users";

		return cache.GetOrCreateAsync(
			cacheKey,
			async entry =>
			{
				var result = await yandexTrackerClient.GetUsersAsync(paginationSettings, cancellationToken);

				entry.SetAbsoluteExpiration(options.CurrentValue.Ttl);

				return result;
			})!;
	}

	public void Dispose()
	{
		yandexTrackerClient.Dispose();
	}
}