using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mindbox.YandexTracker.Template;
using Moq;

namespace Mindbox.YandexTracker.Tests.UnitTests;

[TestClass]
public class YandexTrackerClientCachingDecoratorTests
{
	private Mock<IMemoryCache> _memoryCacheMock = null!;
	private Mock<IYandexTrackerClient> _yandexTrackerClientMock = null!;

	[TestInitialize]
	public void TestInitialize()
	{
		_memoryCacheMock = new Mock<IMemoryCache>();
		_yandexTrackerClientMock = new Mock<IYandexTrackerClient>();
	}

	[TestMethod]
	public async Task CreateAttachmentAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.CreateAttachmentAsync(null!, null!);

		_yandexTrackerClientMock.Verify(x => x.CreateAttachmentAsync(
			It.IsAny<string>(),
			It.IsAny<Stream>(),
			It.IsAny<string>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task CreateCommentAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.CreateCommentAsync(null!, null!);

		_yandexTrackerClientMock.Verify(x => x.CreateCommentAsync(
			It.IsAny<string>(),
			It.IsAny<Comment>(),
			It.IsAny<bool?>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task CreateIssueAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.CreateIssueAsync(null!);

		_yandexTrackerClientMock.Verify(x => x.CreateIssueAsync(
			It.IsAny<Issue>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task CreateProjectAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.CreateProjectAsync(ProjectEntityType.Project, null!);

		_yandexTrackerClientMock.Verify(x => x.CreateProjectAsync(
			It.IsAny<ProjectEntityType>(),
			It.IsAny<Project>(),
			It.IsAny<ProjectFieldData?>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task CreateQueueAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.CreateQueueAsync(null!);

		_yandexTrackerClientMock.Verify(x => x.CreateQueueAsync(
			It.IsAny<Queue>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task DeleteAttachmentAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.DeleteAttachmentAsync(null!, null!);

		_yandexTrackerClientMock.Verify(x => x.DeleteAttachmentAsync(
			It.IsAny<string>(),
			It.IsAny<string>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task DeleteCommentAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.DeleteCommentAsync(null!, 0);

		_yandexTrackerClientMock.Verify(x => x.DeleteCommentAsync(
			It.IsAny<string>(),
			It.IsAny<int>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task DeleteProjectAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.DeleteProjectAsync(ProjectEntityType.Project, 0);

		_yandexTrackerClientMock.Verify(x => x.DeleteProjectAsync(
			It.IsAny<ProjectEntityType>(),
			It.IsAny<int>(),
			It.IsAny<bool?>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task DeleteQueueAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.DeleteQueueAsync(null!);

		_yandexTrackerClientMock.Verify(x => x.DeleteQueueAsync(
			It.IsAny<string>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task GetAccessibleFieldsForIssueAsync_EmptyCache_ValuesTakenFromServer()
	{
		var queueKey = new Fixture().Create<string>();
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock.SetupSequence(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
			.Returns(false)
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		_yandexTrackerClientMock
			.Setup(x => x.GetAccessibleFieldsForIssueAsync(
				queueKey,
				It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetAccessibleFieldsForIssueAsync(queueKey);
		await decorator.GetAccessibleFieldsForIssueAsync(queueKey);

		_yandexTrackerClientMock.Verify(x => x.GetAccessibleFieldsForIssueAsync(
				queueKey,
				It.IsAny<CancellationToken>()),
			Times.Once);
	}

	[TestMethod]
	public async Task GetAccessibleFieldsForIssueAsync_NotEmptyCache_ValuesTakenFromCache()
	{
		object? @null = null;
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock
			.Setup(x => x.TryGetValue(It.IsAny<object>(), out @null))
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetAccessibleFieldsForIssueAsync(null!);

		_memoryCacheMock.Verify(x => x.CreateEntry(It.IsAny<object>()), Times.Never);

		_yandexTrackerClientMock.Verify(x => x.GetAccessibleFieldsForIssueAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[TestMethod]
	public async Task GetAttachmentsAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetAttachmentsAsync(null!);

		_yandexTrackerClientMock.Verify(x => x.GetAttachmentsAsync(
			It.IsAny<string>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task GetCommentsAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetCommentsAsync(null!);

		_yandexTrackerClientMock.Verify(x => x.GetCommentsAsync(
			It.IsAny<string>(),
			It.IsAny<CommentExpandData?>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task GetComponentsAsync_EmptyCache_ValuesTakenFromServer()
	{
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock.SetupSequence(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
			.Returns(false)
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		_yandexTrackerClientMock
			.Setup(x => x.GetComponentsAsync(
				It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetComponentsAsync();
		await decorator.GetComponentsAsync();

		_yandexTrackerClientMock.Verify(x => x.GetComponentsAsync(
				It.IsAny<CancellationToken>()),
			Times.Once);
	}

	[TestMethod]
	public async Task GetComponentsAsync_NotEmptyCache_ValuesTakenFromCache()
	{
		object? @null = null;
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock
			.Setup(x => x.TryGetValue(It.IsAny<object>(), out @null))
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetComponentsAsync();

		_memoryCacheMock.Verify(x => x.CreateEntry(It.IsAny<object>()), Times.Never());

		_yandexTrackerClientMock.Verify(x => x.GetComponentsAsync(
			It.IsAny<CancellationToken>()), Times.Never);
	}

	[TestMethod]
	public async Task GetIssueAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetIssueAsync(null!);

		_yandexTrackerClientMock.Verify(x => x.GetIssueAsync(
			It.IsAny<string>(),
			It.IsAny<IssueExpandData?>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task GetUserByIdAsync_EmptyCache_ValuesTakenFromServer()
	{
		var userId = "123";
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock.SetupSequence(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
			.Returns(false)
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		_yandexTrackerClientMock
			.Setup(x => x.GetUserByIdAsync(
				userId,
				It.IsAny<CancellationToken>()))
			.Returns(Task.FromResult<UserDetailedInfo>(null!));

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetUserByIdAsync(userId);
		await decorator.GetUserByIdAsync(userId);

		_yandexTrackerClientMock.Verify(x => x.GetUserByIdAsync(
				userId,
				It.IsAny<CancellationToken>()),
			Times.Once);
	}

	[TestMethod]
	public async Task GetUserByIdAsync_NotEmptyCache_ValuesTakenFromCache()
	{
		object? @null = null;
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock
			.Setup(x => x.TryGetValue(It.IsAny<object>(), out @null))
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetUserByIdAsync(null!);

		_memoryCacheMock.Verify(x => x.CreateEntry(It.IsAny<object>()), Times.Never());

		_yandexTrackerClientMock.Verify(x => x.GetUserByIdAsync(
			It.IsAny<string>(),
			It.IsAny<CancellationToken>()), Times.Never);
	}

	[TestMethod]
	public async Task GetIssuesByFilterAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetIssuesByFilterAsync(new IssuesFilter());

		_yandexTrackerClientMock.Verify(x => x.GetIssuesByFilterAsync(
			It.IsAny<IssuesFilter>(),
			It.IsAny<IssuesExpandData?>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task GetIssuesByQueryAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetIssuesByQueryAsync(null!);

		_yandexTrackerClientMock.Verify(x => x.GetIssuesByQueryAsync(
			It.IsAny<string>(),
			It.IsAny<IssuesExpandData?>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task GetIssuesFromKeysAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetIssuesByKeysAsync([]);

		_yandexTrackerClientMock.Verify(x => x.GetIssuesByKeysAsync(
			It.IsAny<IReadOnlyList<string>>(),
			It.IsAny<IssuesExpandData?>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task GetIssuesFromQueueAsync_ValuesTakenFromServer()
	{
		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetIssuesFromQueueAsync(null!);

		_yandexTrackerClientMock.Verify(x => x.GetIssuesFromQueueAsync(
			It.IsAny<string>(),
			It.IsAny<IssuesExpandData?>(),
			It.IsAny<CancellationToken>()));
	}

	[TestMethod]
	public async Task GetIssueStatusesAsync_EmptyCache_ValuesTakenFromServer()
	{
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock.SetupSequence(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
			.Returns(false)
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		_yandexTrackerClientMock
			.Setup(x => x.GetIssueStatusesAsync(
				It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetIssueStatusesAsync();
		await decorator.GetIssueStatusesAsync();

		_yandexTrackerClientMock.Verify(x => x.GetIssueStatusesAsync(
				It.IsAny<CancellationToken>()),
			Times.Once);
	}

	[TestMethod]
	public async Task GetIssueStatusesAsync_NotEmptyCache_ValuesTakenFromCache()
	{
		object? @null = null;
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock
			.Setup(x => x.TryGetValue(It.IsAny<object>(), out @null))
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetIssueStatusesAsync();

		_yandexTrackerClientMock.Verify(x => x.GetIssueStatusesAsync(
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[TestMethod]
	public async Task GetIssueTypesAsync_EmptyCache_ValuesTakenFromServer()
	{
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock.SetupSequence(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
			.Returns(false)
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		_yandexTrackerClientMock
			.Setup(x => x.GetIssueTypesAsync(
				It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetIssueTypesAsync();
		await decorator.GetIssueTypesAsync();

		_yandexTrackerClientMock.Verify(x => x.GetIssueTypesAsync(
				It.IsAny<CancellationToken>()),
			Times.Once);
	}

	[TestMethod]
	public async Task GetIssueTypesAsync_NotEmptyCache_ValuesTakenFromCache()
	{
		object? @null = null;
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock
			.Setup(x => x.TryGetValue(It.IsAny<object>(), out @null))
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetIssueTypesAsync();

		_yandexTrackerClientMock.Verify(x => x.GetIssueTypesAsync(
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[TestMethod]
	public async Task GetProjectsAsync_EmptyCache_ValuesTakenFromServer()
	{
		var entityType = new Fixture().Create<ProjectEntityType>();
		var project = new Fixture().Create<Project>();
		var returnedFields = new Fixture().Create<ProjectFieldData?>();
		var input = new Fixture().Create<string?>();
		var orderBy = new Fixture().Create<string?>();
		var orderAscending = new Fixture().Create<bool?>();
		var rootOnly = new Fixture().Create<bool?>();

		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock.SetupSequence(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
			.Returns(false)
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		_yandexTrackerClientMock
			.Setup(x => x.GetProjectsAsync(
				entityType,
				project,
				returnedFields,
				input,
				orderBy,
				orderAscending,
				rootOnly,
				It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetProjectsAsync(
			entityType,
			project,
			returnedFields,
			input,
			orderBy,
			orderAscending,
			rootOnly);

		await decorator.GetProjectsAsync(
			entityType,
			project,
			returnedFields,
			input,
			orderBy,
			orderAscending,
			rootOnly);

		_yandexTrackerClientMock.Verify(x => x.GetProjectsAsync(
				entityType,
				project,
				returnedFields,
				input,
				orderBy,
				orderAscending,
				rootOnly,
				It.IsAny<CancellationToken>()),
			Times.Once);
	}

	[TestMethod]
	public async Task GetProjectsAsync_NotEmptyCache_ValuesTakenFromCache()
	{
		object? @null = null;
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock
			.Setup(x => x.TryGetValue(It.IsAny<object>(), out @null))
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetProjectsAsync(ProjectEntityType.Project, new Project());

		_yandexTrackerClientMock.Verify(x => x.GetProjectsAsync(
				It.IsAny<ProjectEntityType>(),
				It.IsAny<Project>(),
				It.IsAny<ProjectFieldData?>(),
				It.IsAny<string?>(),
				It.IsAny<string?>(),
				It.IsAny<bool?>(),
				It.IsAny<bool?>(),
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[TestMethod]
	public async Task GetQueueAsync_EmptyCache_ValuesTakenFromServer()
	{
		var queueKey = new Fixture().Create<string>();
		var expandData = new Fixture().Create<QueueExpandData?>();
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock.SetupSequence(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
			.Returns(false)
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		_yandexTrackerClientMock
			.Setup(x => x.GetQueueAsync(
				queueKey,
				expandData,
				It.IsAny<CancellationToken>()))
			.Returns(Task.FromResult<Queue>(null!));

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetQueueAsync(queueKey, expandData);
		await decorator.GetQueueAsync(queueKey, expandData);

		_yandexTrackerClientMock.Verify(x => x.GetQueueAsync(
				queueKey,
				expandData,
				It.IsAny<CancellationToken>()),
			Times.Once);
	}

	[TestMethod]
	public async Task GetQueueAsync_NotEmptyCache_ValuesTakenFromCache()
	{
		object? @null = null;
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock
			.Setup(x => x.TryGetValue(It.IsAny<object>(), out @null))
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetQueueAsync(null!);

		_yandexTrackerClientMock.Verify(x => x.GetQueueAsync(
				It.IsAny<string>(),
				It.IsAny<QueueExpandData?>(),
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[TestMethod]
	public async Task GetQueuesAsync_EmptyCache_ValuesTakenFromServer()
	{
		var expandData = new Fixture().Create<QueuesExpandData?>();
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock.SetupSequence(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
			.Returns(false)
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		_yandexTrackerClientMock
			.Setup(x => x.GetQueuesAsync(
				expandData,
				It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetQueuesAsync(expandData);
		await decorator.GetQueuesAsync(expandData);

		_yandexTrackerClientMock.Verify(x => x.GetQueuesAsync(
				expandData,
				It.IsAny<CancellationToken>()),
			Times.Once);
	}

	[TestMethod]
	public async Task GetQueuesAsync_NotEmptyCache_ValuesTakenFromCache()
	{
		object? @null = null;
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock
			.Setup(x => x.TryGetValue(It.IsAny<object>(), out @null))
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetQueuesAsync();

		_yandexTrackerClientMock.Verify(x => x.GetQueuesAsync(
				It.IsAny<QueuesExpandData?>(),
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[TestMethod]
	public async Task GetResolutionsAsync_EmptyCache_ValuesTakenFromServer()
	{
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock.SetupSequence(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
			.Returns(false)
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		_yandexTrackerClientMock
			.Setup(x => x.GetResolutionsAsync(
				It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetResolutionsAsync();
		await decorator.GetResolutionsAsync();

		_yandexTrackerClientMock.Verify(x => x.GetResolutionsAsync(
				It.IsAny<CancellationToken>()),
			Times.Once);
	}

	[TestMethod]
	public async Task GetResolutionsAsync_NotEmptyCache_ValuesTakenFromCache()
	{
		object? @null = null;
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock
			.Setup(x => x.TryGetValue(It.IsAny<object>(), out @null))
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetResolutionsAsync();

		_yandexTrackerClientMock.Verify(x => x.GetResolutionsAsync(
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[TestMethod]
	public async Task GetTagsAsync_EmptyCache_ValuesTakenFromServer()
	{
		var queueKey = new Fixture().Create<string>();
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock.SetupSequence(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
			.Returns(false)
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		_yandexTrackerClientMock
			.Setup(x => x.GetTagsAsync(
				queueKey,
				It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetTagsAsync(queueKey);
		await decorator.GetTagsAsync(queueKey);

		_yandexTrackerClientMock.Verify(x => x.GetTagsAsync(
				queueKey,
				It.IsAny<CancellationToken>()),
			Times.Once);
	}

	[TestMethod]
	public async Task GetTagsAsync_NotEmptyCache_ValuesTakenFromCache()
	{
		object? @null = null;
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock
			.Setup(x => x.TryGetValue(It.IsAny<object>(), out @null))
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetTagsAsync(null!);

		_yandexTrackerClientMock.Verify(x => x.GetTagsAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[TestMethod]
	public async Task GetUsersAsync_EmptyCache_ValuesTakenFromServer()
	{
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock.SetupSequence(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
			.Returns(false)
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		_yandexTrackerClientMock
			.Setup(x => x.GetUsersAsync(
				It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetUsersAsync();
		await decorator.GetUsersAsync();

		_yandexTrackerClientMock.Verify(x => x.GetUsersAsync(
				It.IsAny<CancellationToken>()),
			Times.Once);
	}

	[TestMethod]
	public async Task GetUsersAsync_NotEmptyCache_ValuesTakenFromCache()
	{
		object? @null = null;
		var cacheEntryMock = new Mock<ICacheEntry>();

		_memoryCacheMock
			.Setup(x => x.TryGetValue(It.IsAny<object>(), out @null))
			.Returns(true);

		_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
			.Returns(cacheEntryMock.Object);

		using var decorator = CreateYandexTrackerClientCachingDecorator();

		await decorator.GetUsersAsync();

		_yandexTrackerClientMock.Verify(x => x.GetUsersAsync(
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	private YandexTrackerClientCachingDecorator CreateYandexTrackerClientCachingDecorator()
	{
		return new YandexTrackerClientCachingDecorator(
			_yandexTrackerClientMock.Object,
			_memoryCacheMock.Object,
			new YandexTrackerClientCachingOptions());
	}
}
