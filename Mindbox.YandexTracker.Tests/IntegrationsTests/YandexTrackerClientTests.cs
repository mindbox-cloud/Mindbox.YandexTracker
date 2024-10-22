using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mindbox.YandexTracker.Tests;

[TestClass]
public class YandexTrackerClientTests : YandexTrackerTestBase
{
	private int _index;

	public YandexTrackerClientTests()
	{
		_index = 0;
	}

	[TestMethod]
	public async Task GetQueueAsync_ValidQueueKey_AllFieldsIncludedInResponse_ShouldReturnExistingQueue()
	{
		// Arrange & Act
		var response = await YandexTrackerClient.GetQueueAsync(
			TestQueueKey,
			QueueExpandData.All);

		// Assert
		Assert.IsNotNull(response);
		Assert.AreEqual(TestQueueKey, response.Key);
	}

	[TestMethod]
	public async Task GetQueuesAsync_ResponseInNotNullAndContainsSomeQueues()
	{
		// Arrange & Act
		var response = await YandexTrackerClient.GetQueuesAsync();

		// Assert
		Assert.IsNotNull(response);
		Assert.IsTrue(response.Any());
	}

	[TestMethod]
	public async Task GetIssueAsync_IssueKey_ResponseIsNotNullAndIssueSuccessfullyCreated()
	{
		// Arrange
		var issue = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = GetUniqueName()
		});

		// Act
		var issueResponse = await YandexTrackerClient.GetIssueAsync(issue.Key);

		// Arrange
		Assert.IsNotNull(issue);
		Assert.AreEqual(issue.Key, issueResponse.Key);
	}

	[TestMethod]
	public async Task GetIssueAsync_IssueKeyWhichNotExist_ThrowYandexTrackerExceptionE()
	{
		var notExistedIssueKey = "randomIssue";

		await Assert.ThrowsExceptionAsync<YandexTrackerException>(async () =>
		{
			await YandexTrackerClient.GetIssueAsync(notExistedIssueKey);
		});
	}

	[TestMethod]
	public async Task GetFieldCategoriesAsync_ResponseIsNotNullAndContainsSomeCategories()
	{
		var categories = await YandexTrackerClient.GetFieldCategoriesAsync();

		Assert.IsNotNull(categories);
		Assert.IsTrue(categories.Any());
	}

	[TestMethod]
	public async Task CreateIssueAsync_CustomFields_ShouldCreatedIssueWithCustomFields()
	{
		var firstCategory = (await YandexTrackerClient.GetFieldCategoriesAsync()).ToList().FirstOrDefault()!;

		var customField = await YandexTrackerClient.CreateLocalFieldInQueueAsync(TestQueueKey, new QueueLocalField
		{
			Id = "customFields",
			CategoryId = firstCategory.Id,
			FieldName = new QueueLocalFieldName
			{
				En = "customFields",
				Ru = "Кастомное поле"
			},
			FieldType = QueueLocalFieldType.StringFieldType
		});

		var issue = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = GetUniqueName(),
			CustomFields = new()
			{
				{ customField.Id, "field1" }
			}
		});

		var response = await YandexTrackerClient.GetIssueAsync(issue.Key);

		Assert.IsNotNull(response);
		Assert.IsTrue(response.CustomFields.ContainsKey(customField.Id));
	}

	[TestMethod]
	public async Task CreateLocalFieldInQueueAsync_ValidRequest_ShouldCreatedLocalField()
	{
		var firstCategory = (await YandexTrackerClient.GetFieldCategoriesAsync()).ToList().FirstOrDefault()!;

		var localField = new QueueLocalField
		{
			Id = "someId",
			FieldName = new QueueLocalFieldName()
			{
				En = "eng222",
				Ru = "ru222"
			},
			CategoryId = firstCategory.Id,
			FieldType = QueueLocalFieldType.DateFieldType
		};

		await Task.Delay(1000); // Чтобы поле точно создалось в трекере

		var response = await YandexTrackerClient.CreateLocalFieldInQueueAsync(TestQueueKey, localField);

		Assert.IsNotNull(response);
		Assert.IsTrue(response.Id.EndsWith(localField.Id, StringComparison.InvariantCulture));
	}

	[TestMethod]
	public async Task GetIssuesFromQueueAsync_ValidQueueKey_ResponseIsNotNullAndIssueKeysAreEqual()
	{
		var issue1 = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = GetUniqueName()
		});

		var issue2 = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = GetUniqueName()
		});

		await Task.Delay(1000);  // Чтобы задачи точно создались в трекере

		var issues = await YandexTrackerClient.GetIssuesFromQueueAsync(TestQueueKey);

		Assert.IsNotNull(issues);
		Assert.IsTrue(issues.Count >= 2);
		Assert.IsTrue(issues.Any(issue => issue.Key == issue1.Key));
		Assert.IsTrue(issues.Any(issue => issue.Key == issue2.Key));
	}

	[TestMethod]
	public async Task GetIssuesByKeysAsync_ValidIssueKey_ResponseIsNotNullAndIssueKeysAreEqual()
	{
		var issue1 = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = GetUniqueName()
		});

		var issue2 = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = GetUniqueName()
		});

		await Task.Delay(1000);  // Чтобы задачи точно создались в трекере

		var issues = await YandexTrackerClient.GetIssuesByKeysAsync([issue1.Key, issue2.Key]);

		Assert.IsNotNull(issues);
		// могут быть задачи, созданные в других тестах, поэтому проверяем через any
		Assert.AreEqual(2, issues.Count);
		Assert.IsTrue(issues.Any(issue => issue.Key == issue1.Key));
		Assert.IsTrue(issues.Any(issue => issue.Key == issue2.Key));
	}

	[TestMethod]
	public async Task GetIssuesByFilterAsync_ValidFilter_ResponseIsNotNullAndIssueKeysAreEqual()
	{
		var summary = GetUniqueName();

		await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = GetUniqueName(),
		});

		var issue = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = summary,
		});

		await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = GetUniqueName()
		});

		await Task.Delay(1000);  // Чтобы задачи точно создались в трекере

		var issues = await YandexTrackerClient.GetIssuesByFilterAsync(new IssuesFilter
		{
			Queue = TestQueueKey,
			Summary = summary
		});

		Assert.IsNotNull(issues);
		// могут быть задачи, созданные в других тестах, поэтому проверяем через any
		Assert.IsTrue(issues.Count >= 1);
		Assert.IsTrue(issues.Any(@issue => @issue.Key == issue.Key));
	}

	[TestMethod]
	public async Task GetIssuesByQueryAsync_ValidQuery_ResponseIsNotNullAndIssueKeysAreEqual()
	{
		var summary1 = GetUniqueName();
		var summary2 = GetUniqueName();
		var issue1 = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = summary1
		});

		var issue2 = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = summary2
		});

		await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = GetUniqueName()
		});

		await Task.Delay(1000);  // Чтобы задачи точно создались в трекере

		var issues = await YandexTrackerClient.GetIssuesByQueryAsync(
			$"Queue: {TestQueueKey} AND Summary: {summary1}, {summary2} \"Sort By\": Summary");

		Assert.IsNotNull(issues);
		// могут быть задачи, созданные в других тестах, поэтому проверяем через any
		Assert.IsTrue(issues.Count >= 2);
		Assert.IsTrue(issues.Any(issue => issue.Key == issue1.Key));
		Assert.IsTrue(issues.Any(issue => issue.Key == issue2.Key));
	}

	[TestMethod]
	public async Task GetComponentsAsync_ResponseIsNotNullAndNotEmpty()
	{
		var component1 = await YandexTrackerClient.CreateComponentAsync(GetUniqueName(), TestQueueKey);
		var component2 = await YandexTrackerClient.CreateComponentAsync(GetUniqueName(), TestQueueKey);

		await Task.Delay(1000); // Чтобы компоненты точно создались в трекере

		var components = (await YandexTrackerClient.GetComponentsAsync()).ToArray();

		Assert.IsNotNull(components);
		Assert.AreNotEqual(component1.Id, component2.Id);
		// могут быть какие-то дефолтные, поэтому проверяем через any
		Assert.IsTrue(components.Any(component => component.Id == component1.Id));
		Assert.IsTrue(components.Any(component => component.Id == component2.Id));
	}

	[TestMethod]
	public async Task GetCommentsAsync_IssueKey_ResponseIsNotNullAndNotEmpty()
	{
		var issue = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = GetUniqueName()
		});

		var comment1 = await YandexTrackerClient.CreateCommentAsync(
			issue.Key,
			new Comment
			{
				Text = GetUniqueName()
			});

		var comment2 = await YandexTrackerClient.CreateCommentAsync(
			issue.Key,
			new Comment
			{
				Text = GetUniqueName()
			});

		await Task.Delay(1000); // Чтобы компоненты и задача точно создались в трекере

		var comments = (await YandexTrackerClient.GetCommentsAsync(issue.Key))
			.OrderBy(comment => comment.CreatedAtUtc)
			.ToArray();

		Assert.IsNotNull(comments);
		Assert.AreEqual(2, comments.Length);
		Assert.AreEqual(comment1.Id, comments.First().Id);
		Assert.AreEqual(comment2.Id, comments.Last().Id);
	}

	[TestMethod]
	public async Task GetAttachmentsAsync_IssueKey_ResponseIsNotNullAndNotEmpty()
	{
		var issue = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			QueueKey = TestQueueKey,
			Summary = GetUniqueName()
		});

		await using var imageFile = File.OpenRead(Path.Combine("TestFiles", "pepe.png"));
		await using var txtFile = File.OpenRead(Path.Combine("TestFiles", "importantInformation.txt"));

		var imageAttachment = await YandexTrackerClient.CreateAttachmentAsync(
			issue.Key,
			imageFile);

		var textAttachment = await YandexTrackerClient.CreateAttachmentAsync(
			issue.Key,
			txtFile);

		await Task.Delay(1000); // Чтобы вложения точно создались в трекере

		Attachment[] expectedAttachments = [imageAttachment, textAttachment];

		var attachments = (await YandexTrackerClient.GetAttachmentsAsync(issue.Key)).ToArray();

		var deleteImageAttachmentTask = YandexTrackerClient.DeleteAttachmentAsync(issue.Key, imageAttachment.Id);
		var deleteTxtAttachmentTask = YandexTrackerClient.DeleteAttachmentAsync(issue.Key, textAttachment.Id);

		await Task.WhenAll(deleteImageAttachmentTask, deleteTxtAttachmentTask);

		Assert.IsNotNull(attachments);
		Assert.AreEqual(expectedAttachments.Length, attachments.Length);
	}

	[TestMethod]
	public async Task GetTagsAsync_ResponseIsNotNull()
	{
		var response = await YandexTrackerClient.GetTagsAsync(TestQueueKey);

		Assert.IsNotNull(response);
	}

	[TestMethod]
	public async Task GetProjectsAsync_Project_ProjectFields_ResponseIsNotNullAndContainsOneCreatedProjects()
	{
		// Arrange
		var summary = GetUniqueName();

		var project1 = await YandexTrackerClient.CreateProjectAsync(
			ProjectEntityType.Project,
			new Project
			{
				Summary = summary
			});

		var project2 = await YandexTrackerClient.CreateProjectAsync(
			ProjectEntityType.Project,
			new Project
			{
				Summary = GetUniqueName()
			});

		await Task.Delay(1000); // Чтобы проекты точно создались в трекере

		var requestProject = new Project
		{
			Summary = summary
		};

		// Act
		var projects = await YandexTrackerClient.GetProjectsAsync(
			ProjectEntityType.Project,
			requestProject);

		await YandexTrackerClient.DeleteProjectAsync(ProjectEntityType.Project, project1.ShortId, true);
		await YandexTrackerClient.DeleteProjectAsync(ProjectEntityType.Project, project2.ShortId, true);

		// Assert
		Assert.IsNotNull(projects);

		Assert.IsTrue(projects.Any(project => project.ShortId == project1.ShortId));
	}

	[TestMethod]
	public async Task GetMyselfAsync_ResponseIsNotNull()
	{
		// Arrange & Act
		var userInfo = await YandexTrackerClient.GetMyselfAsync();

		// Assert
		Assert.IsNotNull(userInfo);
	}

	[TestMethod]
	public async Task GetUserByIdAsync_ResponseIsNotNullAndContainsInformationAboutUser()
	{
		// Arrange & Act
		var userInfo = await YandexTrackerClient.GetUserByIdAsync(CurrentUserId);

		// Assert
		Assert.IsNotNull(userInfo);
		Assert.AreEqual(CurrentUserLogin, userInfo.Login);
	}

	[TestMethod]
	public async Task GetUsersAsync_ResponseIsNotNullAndUsersCountGreaterThanZero()
	{
		// Arrange & Act
		var response = await YandexTrackerClient.GetUsersAsync();

		// Assert
		Assert.IsNotNull(response);
		Assert.IsTrue(response.Any());
	}

	[TestMethod]
	public async Task GetResolutionsAsync_ResponseIsNotNull()
	{
		// Arrange & Act
		var response = await YandexTrackerClient.GetResolutionsAsync();

		// Assert
		Assert.IsNotNull(response);
	}

	[TestMethod]
	public async Task GetIssueTypesAsync_ResponseIsNotNull()
	{
		// Arrange & Act
		var response = await YandexTrackerClient.GetIssueTypesAsync();

		// Assert
		Assert.IsNotNull(response);
	}

	[TestMethod]
	public async Task GetIssueStatusesAsync_ResponseIsNotNull()
	{
		// Arrange & Act
		var response = await YandexTrackerClient.GetIssueStatusesAsync();

		// Assert
		Assert.IsNotNull(response);
	}

	[TestMethod]
	public async Task GetAccessibleFieldsForIssueAsync_ValidQueueKey_ResponseIsNotNullAndContainsSomeElements()
	{
		// Arrange & Act
		var response = await YandexTrackerClient.GetAccessibleFieldsForIssueAsync(TestQueueKey);

		// Assert
		Assert.IsNotNull(response);
		Assert.IsTrue(response.Any());
	}

	private string GetUniqueName()
	{
		return $"Testik{_index++}";
	}
}