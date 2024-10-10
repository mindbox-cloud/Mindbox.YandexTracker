using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mindbox.YandexTracker.Tests;

[TestClass]
public class YandexTrackerClientTests : YandexTrackerTestBase
{
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
			Queue = TestQueueKey,
			Summary = "Testik"
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
	public async Task GetIssuesFromQueueAsync_ValidQueueKey_ResponseIsNotNullAndIssueKeysAreEqual()
	{
		var issue1 = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			Queue = TestQueueKey,
			Summary = "Testik1"
		});

		var issue2 = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			Queue = TestQueueKey,
			Summary = "Testik2"
		});

		await Task.Delay(1000);  // Чтобы задачи точно создались в трекере

		var issues = await YandexTrackerClient.GetIssuesFromQueueAsync(TestQueueKey);

		Assert.IsNotNull(issues);
		Assert.AreEqual(issue1.Key, issues[0].Key);
		Assert.AreEqual(issue2.Key, issues[1].Key);
	}

	[TestMethod]
	public async Task GetIssuesFromKeysAsync_ValidIssueKey_ResponseIsNotNullAndIssueKeysAreEqual()
	{
		var issue1 = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			Queue = TestQueueKey,
			Summary = "Testik1"
		});

		var issue2 = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			Queue = TestQueueKey,
			Summary = "Testik2"
		});

		await Task.Delay(1000);  // Чтобы задачи точно создались в трекере

		var issues = await YandexTrackerClient.GetIssuesFromKeysAsync([issue1.Key, issue2.Key]);

		Assert.IsNotNull(issues);
		Assert.AreEqual(issue1.Key, issues[0].Key);
		Assert.AreEqual(issue2.Key, issues[1].Key);
	}

	[TestMethod]
	public async Task GetIssuesByFilterAsync_ValidFilter_ResponseIsNotNullAndIssueKeysAreEqual()
	{
		await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			Queue = TestQueueKey,
			Summary = "Testik1",
		});

		var issue = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			Queue = TestQueueKey,
			Summary = "Testik2",
		});

		await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			Queue = TestQueueKey,
			Summary = "BadTestik"
		});

		await Task.Delay(1000);  // Чтобы задачи точно создались в трекере

		var issues = await YandexTrackerClient.GetIssuesByFilterAsync(new IssuesFilter
		{
			Queue = TestQueueKey,
			Summary = "Testik2"
		});

		Assert.IsNotNull(issues);
		Assert.AreEqual(1, issues.Count);
		Assert.AreEqual(issue.Key, issues[0].Key);
	}

	[TestMethod]
	public async Task GetIssuesByQueryAsync_ValidQuery_ResponseIsNotNullAndIssueKeysAreEqual()
	{
		var issue1 = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			Queue = TestQueueKey,
			Summary = "Testik1"
		});

		var issue2 = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			Queue = TestQueueKey,
			Summary = "Testik2",
		});

		await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			Queue = TestQueueKey,
			Summary = "BadTestik"
		});

		await Task.Delay(1000);  // Чтобы задачи точно создались в трекере

		var issues = await YandexTrackerClient.GetIssuesByQueryAsync(
			$"Queue: {TestQueueKey} AND Summary: Testik1, Testik2 \"Sort By\": Summary");

		Assert.IsNotNull(issues);
		Assert.AreEqual(2, issues.Count);
		Assert.AreEqual(issue1.Key, issues[0].Key);
		Assert.AreEqual(issue2.Key, issues[1].Key);
	}

	[TestMethod]
	public async Task GetComponentsAsync_ResponseIsNotNullAndNotEmpty()
	{
		var components = await YandexTrackerClient.GetComponentsAsync();

		Assert.IsNotNull(components);
		Assert.IsTrue(components.Any());
	}

	[TestMethod]
	public async Task GetCommentsAsync_IssueKey_ResponseIsNotNullAndNotEmpty()
	{
		var issue = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			Queue = TestQueueKey,
			Summary = "Testik1"
		});

		var comment1 = await YandexTrackerClient.CreateCommentAsync(
			issue.Key,
			new Comment
			{
				Text = "SomeComment1"
			});

		var comment2 = await YandexTrackerClient.CreateCommentAsync(
			issue.Key,
			new Comment
			{
				Text = "SomeComment2"
			});

		var comments = (await YandexTrackerClient.GetCommentsAsync(issue.Key)).ToArray();

		Assert.IsNotNull(comments);
		Assert.AreEqual(comment1.Id, comments.First().Id);
		Assert.AreEqual(comment2.Id, comments.Last().Id);
	}

	[TestMethod]
	public async Task GetAttachmentsAsync_IssueKey_ResponseIsNotNullAndNotEmpty()
	{
		var issue = await YandexTrackerClient.CreateIssueAsync(new Issue
		{
			Queue = TestQueueKey,
			Summary = "Testik1"
		});

		await using var imageFile = File.OpenRead(Path.Combine("TestFiles", "pepe.png"));
		await using var txtFile = File.OpenRead(Path.Combine("TestFiles", "importantInformation.txt"));

		var imageAttachment = await YandexTrackerClient.CreateAttachmentAsync(
			issue.Key,
			imageFile);

		var textAttachment = await YandexTrackerClient.CreateAttachmentAsync(
			issue.Key,
			txtFile);

		Attachment[] expectedAttachments = [imageAttachment, textAttachment];

		var attachments = (await YandexTrackerClient.GetAttachmentsAsync(issue.Key)).ToArray();

		var deleteImageAttachmentTask = YandexTrackerClient.DeleteAttachmentAsync(issue.Key, imageAttachment.Id);
		var deleteTxtAttachmentTask = YandexTrackerClient.DeleteAttachmentAsync(issue.Key, textAttachment.Id);

		await Task.WhenAll(deleteImageAttachmentTask, deleteTxtAttachmentTask);

		Assert.IsNotNull(attachments);
		Assert.AreEqual(2, attachments.Length);
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
		var project1 = await YandexTrackerClient.CreateProjectAsync(
			ProjectEntityType.Project,
			new Project
			{
				Summary = "project1"
			});

		var project2 = await YandexTrackerClient.CreateProjectAsync(
			ProjectEntityType.Project,
			new Project
			{
				Summary = "project2"
			});

		await Task.Delay(1000); // Чтобы проекты точно создались в трекере

		var requestProject = new Project
		{
			Summary = "project1"
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
}