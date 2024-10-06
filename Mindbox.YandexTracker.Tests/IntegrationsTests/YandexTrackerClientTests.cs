using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mindbox.YandexTracker.Tests;

[TestClass]
public class YandexTrackerClientTests : YandexTrackerTestBase
{
	[TestMethod]
	public async Task GetQuqueAsync_ValidQueueKey_AllFieldsIncludedInResponse_ShouldReturnExistingQuque()
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
	public async Task GetIssueAsync_IssueKey_ResponseIsNotNull()
	{
		// Arrange
		var issue = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = "Testik"
		});

		// Act
		var issueResponse = await YandexTrackerClient.GetIssueAsync(issue.Key);

		// Arrange
		Assert.IsNotNull(issue);
		Assert.AreEqual(issue, issueResponse);
	}

	[TestMethod]
	public async Task GetIssueAsync_IssueKeyWhichNotExist_ThrowException()
	{
		var someIssueKey = "randomIssue";

		await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
		{
			await YandexTrackerClient.GetIssueAsync(someIssueKey);
		});
	}

	[TestMethod]
	public async Task GetIssuesAsync_ResponseContainsTwoIssues()
	{
		var issue1 = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = "Testik1"
		});

		var issue2 = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = "Testik2"
		});

		Issue[] expectedIssues = [issue1, issue2];

		var issues = (await YandexTrackerClient.GetIssuesAsync(new GetIssuesRequest
		{
			Queue = TestQueueKey
		}))
		.ToArray();

		CollectionAssert.AreEquivalent(expectedIssues, issues);
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
		var issue = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = "Testik1"
		});

		var comment1 = await YandexTrackerClient.CreateCommentAsync(
			issue.Key,
			new CreateCommentRequest
			{
				Text = "SomeComment1"
			});

		var comment2 = await YandexTrackerClient.CreateCommentAsync(
			issue.Key,
			new CreateCommentRequest
			{
				Text = "SomeComment2"
			});

		Comment[] expectedComments = [comment1, comment2];

		var comments = (await YandexTrackerClient.GetCommentsAsync(issue.Key)).ToArray();

		Assert.IsNotNull(comments);
		CollectionAssert.AreEquivalent(expectedComments, comments);

		var deleteFirstCommentTask = YandexTrackerClient.DeleteCommentAsync(issue.Key, comment1.Id);
		var deleteSecondCommentTask = YandexTrackerClient.DeleteCommentAsync(issue.Key, comment2.Id);

		await Task.WhenAll(deleteFirstCommentTask, deleteSecondCommentTask);
	}

	[TestMethod]
	public async Task GetAttachmentsAsync_IssueKey_ResponseIsNotNullAndNotEmpty()
	{
		var issue = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = "Testik1"
		});

		var attachment1 = await YandexTrackerClient.CreateAttachmentAsync(
			issue.Key,
			null!);

		var attachment2 = await YandexTrackerClient.CreateAttachmentAsync(
			issue.Key,
			null!);

		Attachment[] expectedAttachments = [attachment1, attachment2];

		var attachments = (await YandexTrackerClient.GetAttachmentsAsync(issue.Key)).ToArray();

		Assert.IsNotNull(attachments);
		CollectionAssert.AreEquivalent(expectedAttachments, attachments);
	}

	[TestMethod]
	public async Task GetTagsAsync_ResponseIsNotNull()
	{
		var response = await YandexTrackerClient.GetTagsAsync(TestQueueKey);

		Assert.IsNotNull(response);
	}

	[TestMethod]
	public async Task GetProjectsAsync_Project_ProjectFields_ResponseIsNotNullAndContainsTwoProjects()
	{
		// Arrange
		var project1 = await YandexTrackerClient.CreateProjectAsync(
			ProjectEntityType.Project,
			new CreateProjectRequest
			{
				Fields = new ProjectFieldsDto
				{
					Summary = "project1"
				}
			});

		var project2 = await YandexTrackerClient.CreateProjectAsync(
			ProjectEntityType.Project,
			new CreateProjectRequest
			{
				Fields = new ProjectFieldsDto
				{
					Summary = "project1"
				}
			});

		Project[] expectedProjects = [project1, project2];

		var request = new GetProjectsRequest
		{
			FieldsWhichIncludedInResponse = ProjectFieldData.None
		};

		// Act
		var projects = (await YandexTrackerClient.GetProjectsAsync(
			ProjectEntityType.Project,
			request))
			.ToArray();

		// Assert
		Assert.IsNotNull(projects);
		CollectionAssert.AreEquivalent(expectedProjects, projects);

		await YandexTrackerClient.DeleteProjectAsync(ProjectEntityType.Project, project1.ShortId);
		await YandexTrackerClient.DeleteProjectAsync(ProjectEntityType.Project, project2.ShortId);
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