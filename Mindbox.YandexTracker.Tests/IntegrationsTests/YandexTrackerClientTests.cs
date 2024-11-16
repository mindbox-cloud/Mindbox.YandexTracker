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
using System.Globalization;
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
		Assert.IsTrue(response.Values.Any());
	}

	[TestMethod]
	public async Task GetIssueAsync_IssueKey_ResponseIsNotNullAndIssueSuccessfullyCreated()
	{
		// Arrange
		var nowUtc = DateTime.UtcNow;
		var dateTimeOnlyStart = DateOnly.FromDateTime(nowUtc);
		var tags = new[] { "tag1", "tag2" };
		var summary = GetUniqueName();

		var issueTypes = await YandexTrackerClient.GetIssueTypesAsync();
		var issueTypeKey = issueTypes.Values.First(x => x.Key == "task").Id;

		var issue = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = summary,
			Description = "DESC",
			Tags = tags,
			Followers = [CurrentUserLogin],
			Assignee = CurrentUserLogin,
			Author = CurrentUserLogin,
			Priority = Priority.Trivial,
			Start = dateTimeOnlyStart,
			Type = issueTypeKey
		});

		// Act
		var issueResponse = await YandexTrackerClient.GetIssueAsync(issue.Key);

		// Arrange
		Assert.IsNotNull(issue);
		Assert.AreEqual(issue.Key, issueResponse.Key);
		Assert.AreEqual(summary, issueResponse.Summary);
		Assert.AreEqual(dateTimeOnlyStart, issueResponse.Start);
		Assert.AreEqual("task", issueResponse.Type.Key);
		Assert.AreEqual("DESC", issueResponse.Description);
		Assert.AreEqual(Priority.Trivial.ToString().ToUpperInvariant(), issueResponse.Priority.Key!.ToUpperInvariant());
		Assert.AreEqual(CurrentUserId, issueResponse.CreatedBy.Id);
		Assert.AreEqual(CurrentUserId, issueResponse.UpdatedBy.Id);
		Assert.AreEqual(CurrentUserId, issueResponse.Assignee!.Id);
		Assert.AreEqual(1, issueResponse.Followers.Count);
		Assert.AreEqual(CurrentUserId, issueResponse.Followers.First().Id);
		Assert.AreEqual(2, issueResponse.Tags.Count);
		CollectionAssert.AreEqual(tags, issueResponse.Tags.ToList());
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
		Assert.IsTrue(categories.Values.Any());
	}

	[TestMethod]
	public async Task CreateIssueAsync_CustomFields_ShouldCreatedIssueWithCustomFields()
	{
		var firstCategory = (await YandexTrackerClient.GetFieldCategoriesAsync()).Values[0];

		var customField = await YandexTrackerClient.CreateLocalFieldInQueueAsync(TestQueueKey, new CreateQueueLocalFieldRequest
		{
			Id = "customFields",
			Category = firstCategory.Id,
			Name = new QueueLocalFieldName
			{
				En = "customFields",
				Ru = "Кастомное поле"
			},
			Type = QueueLocalFieldType.StringFieldType
		});

		await Task.Delay(TimeSpan.FromSeconds(2)); // Чтобы поле точно создалось в трекере

		var issue = new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName(),
			Project = TestProjectShortId
		};

		issue.SetCustomField<string>(customField.Id, "field1");

		var createdIssue = await YandexTrackerClient.CreateIssueAsync(issue);

		var response = await YandexTrackerClient.GetIssueAsync(createdIssue.Key);

		Assert.IsNotNull(response);
		Assert.AreEqual("field1", response.GetCustomField<string>(customField.Id));
	}

	[TestMethod]
	public async Task ImportIssueAsync_CustomFields_ShouldImportIssueWithCustomFields()
	{
		var firstCategory = (await YandexTrackerClient.GetFieldCategoriesAsync()).Values[0];
		var resolutions = await YandexTrackerClient.GetResolutionsAsync();
		var expectedResolution = resolutions.Values[0];

		var customField = await YandexTrackerClient.CreateLocalFieldInQueueAsync(TestQueueKey, new CreateQueueLocalFieldRequest
		{
			Id = "customFieldsForImpor",
			Category = firstCategory.Id,
			Name = new QueueLocalFieldName
			{
				En = "customFieldsForImport",
				Ru = "Кастомное поле для импорта"
			},
			Type = QueueLocalFieldType.StringFieldType
		});

		await Task.Delay(TimeSpan.FromSeconds(2)); // Чтобы поле точно создалось в трекере

		var createdAt = new DateTime(2021, 10, 10, 10, 10, 10);
		var resolvedAt = createdAt.AddMinutes(5);
		var updatedAt = createdAt.AddMinutes(10);
		var importIssueRequest = new ImportIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName(),
			Project = TestProjectShortId,
			StoryPoints = 1.5,

			CreatedBy = CurrentUserId,
			CreatedAt = createdAt,

			UpdatedAt = updatedAt,
			UpdatedBy = CurrentUserId,

			ResolvedAt = resolvedAt,
			ResolvedBy = CurrentUserLogin,
			Resolution = expectedResolution.Id,
		};

		importIssueRequest.SetCustomField<string>(customField.Id, "field1");

		var importedIssue = await YandexTrackerClient.ImportIssueAsync(importIssueRequest);

		var issue = await YandexTrackerClient.GetIssueAsync(importedIssue.Key);

		Assert.IsNotNull(issue);
		Assert.AreEqual(createdAt, issue.CreatedAt);
		Assert.IsNotNull(issue.CreatedBy);
		Assert.AreEqual(CurrentUserId, issue.CreatedBy.Id);
		Assert.AreEqual(updatedAt, issue.UpdatedAt);
		Assert.IsNotNull(issue.UpdatedBy);
		Assert.AreEqual(CurrentUserId, issue.UpdatedBy.Id);
		Assert.AreEqual(resolvedAt, issue.ResolvedAt);
		Assert.IsNotNull(issue.ResolvedBy);
		Assert.AreEqual(CurrentUserId, issue.ResolvedBy.Id);
		Assert.IsNotNull(issue.Resolution);
		Assert.AreEqual(expectedResolution.Id.ToString(CultureInfo.InvariantCulture), issue.Resolution!.Id);
		Assert.IsNull(issue.Author);
		Assert.IsNull(issue.Parent);
		Assert.IsTrue(issue.Sprint.Count == 0);
		Assert.AreEqual(importIssueRequest.StoryPoints, issue.StoryPoints);

		Assert.AreEqual("field1", issue.GetCustomField<string>(customField.Id));
	}

	[TestMethod]
	public async Task CreateLocalFieldInQueueAsync_ValidRequest_ShouldCreatedLocalField()
	{
		var firstCategory = (await YandexTrackerClient.GetFieldCategoriesAsync()).Values[0];

		var localField = new CreateQueueLocalFieldRequest
		{
			Id = "someId",
			Name = new QueueLocalFieldName
			{
				En = "eng222",
				Ru = "ru222"
			},
			Category = firstCategory.Id,
			Type = QueueLocalFieldType.DateFieldType
		};

		await Task.Delay(1000); // Чтобы поле точно создалось в трекере

		var response = await YandexTrackerClient.CreateLocalFieldInQueueAsync(TestQueueKey, localField);

		Assert.IsNotNull(response);
		Assert.IsTrue(response.Id.EndsWith(localField.Id, StringComparison.InvariantCulture));
	}

	[TestMethod]
	public async Task GetIssuesFromQueueAsync_ValidQueueKey_ResponseIsNotNullAndIssueKeysAreEqual()
	{
		var issue1 = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName()
		});

		var issue2 = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName()
		});

		await Task.Delay(1000);  // Чтобы задачи точно создались в трекере

		var issues = await YandexTrackerClient.GetIssuesFromQueueAsync(TestQueueKey);

		Assert.IsNotNull(issues);
		Assert.IsTrue(issues.Values.Count >= 2);
		Assert.IsTrue(issues.Values.Any(issue => issue.Key == issue1.Key));
		Assert.IsTrue(issues.Values.Any(issue => issue.Key == issue2.Key));
	}

	[TestMethod]
	public async Task GetIssuesByKeysAsync_ValidIssueKey_ResponseIsNotNullAndIssueKeysAreEqual()
	{
		var issue1 = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName()
		});

		var issue2 = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName()
		});

		await Task.Delay(1000);  // Чтобы задачи точно создались в трекере

		var issues = await YandexTrackerClient.GetIssuesByKeysAsync([issue1.Key, issue2.Key]);

		Assert.IsNotNull(issues);
		// могут быть задачи, созданные в других тестах, поэтому проверяем через any
		Assert.AreEqual(2, issues.Values.Count);
		Assert.IsTrue(issues.Values.Any(issue => issue.Key == issue1.Key));
		Assert.IsTrue(issues.Values.Any(issue => issue.Key == issue2.Key));
	}

	[TestMethod]
	public async Task GetIssuesByFilterAsync_ValidFilter_ResponseIsNotNullAndIssueKeysAreEqual()
	{
		var summary = GetUniqueName();

		await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName(),
		});

		var issue = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = summary,
		});

		await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName()
		});

		await Task.Delay(1000);  // Чтобы задачи точно создались в трекере

		var issues = await YandexTrackerClient.GetIssuesByFilterAsync(new GetIssuesByFilterRequest
		{
			Filter = new IssuesFilterDto
			{
				Queue = [TestQueueKey],
				Summary = [summary]
			}
		});

		Assert.IsNotNull(issues);
		// могут быть задачи, созданные в других тестах, поэтому проверяем через any
		Assert.IsTrue(issues.Values.Count >= 1);
		Assert.IsTrue(issues.Values.Any(@issue => @issue.Key == issue.Key));
	}

	[TestMethod]
	public async Task GetIssuesByQueryAsync_ValidQuery_ResponseIsNotNullAndIssueKeysAreEqual()
	{
		var summary1 = GetUniqueName();
		var summary2 = GetUniqueName();
		var issue1 = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = summary1
		});

		var issue2 = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = summary2
		});

		await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName()
		});

		await Task.Delay(1000);  // Чтобы задачи точно создались в трекере

		var issues = await YandexTrackerClient.GetIssuesByQueryAsync(
			$"Queue: {TestQueueKey} AND Summary: {summary1}, {summary2} \"Sort By\": Summary");

		Assert.IsNotNull(issues);
		// могут быть задачи, созданные в других тестах, поэтому проверяем через any
		Assert.IsTrue(issues.Values.Count >= 2);
		Assert.IsTrue(issues.Values.Any(issue => issue.Key == issue1.Key));
		Assert.IsTrue(issues.Values.Any(issue => issue.Key == issue2.Key));
	}

	[Ignore("Компоненты нельзя удалить, вместе с очередью они почему-то не удаляются, запускать только вручную")]
	[TestMethod]
	public async Task GetComponentsAsync_ResponseIsNotNullAndNotEmpty()
	{
		var component1 = await YandexTrackerClient.CreateComponentAsync(
			new CreateComponentRequest()
			{
				Name = GetUniqueName(),
				Queue = TestQueueKey
			});
		var component2 = await YandexTrackerClient.CreateComponentAsync(
			new CreateComponentRequest()
			{
				Name = GetUniqueName(),
				Queue = TestQueueKey
			});

		await Task.Delay(1000); // Чтобы компоненты точно создались в трекере

		var components = (await YandexTrackerClient.GetComponentsAsync()).Values;

		Assert.IsNotNull(components);
		Assert.AreNotEqual(component1.Id, component2.Id);
		// могут быть какие-то дефолтные, поэтому проверяем через any
		Assert.IsTrue(components.Any(component => component.Id == component1.Id));
		Assert.IsTrue(components.Any(component => component.Id == component2.Id));
	}

	[TestMethod]
	public async Task GetCommentsAsync_IssueKey_ResponseIsNotNullAndNotEmpty()
	{
		var issue = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName()
		});

		var comment1 = await YandexTrackerClient.CreateCommentAsync(
			issue.Key,
			new CreateCommentRequest
			{
				Text = GetUniqueName()
			});

		var comment2 = await YandexTrackerClient.CreateCommentAsync(
			issue.Key,
			new CreateCommentRequest
			{
				Text = GetUniqueName()
			});

		await Task.Delay(1000); // Чтобы компоненты и задача точно создались в трекере

		var comments = (await YandexTrackerClient.GetCommentsAsync(issue.Key))
			.Values
			.OrderBy(comment => comment.CreatedAt)
			.ToArray();

		Assert.IsNotNull(comments);
		Assert.AreEqual(2, comments.Length);
		Assert.AreEqual(comment1.Id, comments.First().Id);
		Assert.AreEqual(comment2.Id, comments.Last().Id);
	}

	[TestMethod]
	public async Task GetAttachmentsAsync_IssueKey_ResponseIsNotNullAndNotEmpty()
	{
		var issue = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
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

		await Task.Delay(3000); // Чтобы вложения точно создались в трекере

		CreateAttachmentResponse[] expectedAttachments = [imageAttachment, textAttachment];

		var attachments = await YandexTrackerClient.GetAttachmentsAsync(issue.Key);

		var deleteImageAttachmentTask = YandexTrackerClient.DeleteAttachmentAsync(issue.Key, imageAttachment.Id);
		var deleteTxtAttachmentTask = YandexTrackerClient.DeleteAttachmentAsync(issue.Key, textAttachment.Id);

		await Task.WhenAll(deleteImageAttachmentTask, deleteTxtAttachmentTask);

		Assert.IsNotNull(attachments);
		Assert.AreEqual(expectedAttachments.Length, attachments.Values.Count);
	}

	[TestMethod]
	public async Task CreateTemporaryAttachmentAsync_SomeTemporaryFiles_ResponseIsNotNullAndNotEmptyAndCommentsHasAttachments()
	{
		var issue = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName()
		});

		await using var imageFile = File.OpenRead(Path.Combine("TestFiles", "pepe.png"));
		await using var txtFile = File.OpenRead(Path.Combine("TestFiles", "importantInformation.txt"));

		var imageAttachment = await YandexTrackerClient.CreateTemporaryAttachmentAsync(imageFile, "pepe.png");
		var textAttachment = await YandexTrackerClient.CreateTemporaryAttachmentAsync(txtFile, "info.txt");

		await YandexTrackerClient.CreateCommentAsync(issue.Key, new CreateCommentRequest
		{
			Text = "This comment has image",
			AttachmentIds = [imageAttachment.Id]
		});

		await YandexTrackerClient.CreateCommentAsync(issue.Key, new CreateCommentRequest
		{
			Text = "This comment has file",
			AttachmentIds = [textAttachment.Id]
		});

		await Task.Delay(1000); // Чтобы комментарии точно создались в трекере

		var comments = (await YandexTrackerClient.GetCommentsAsync(issue.Key, CommentExpandData.Attachments)).Values;

		Assert.IsNotNull(comments);
		Assert.AreEqual(2, comments.Count);
		Assert.AreEqual(imageAttachment.Id, comments[0].Attachments.First().Id);
		Assert.AreEqual(textAttachment.Id, comments[^1].Attachments.First().Id);
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

		var currentUserShortInfo = new UserShortInfoDto()
		{
			Display = CurrentUserLogin,
			Id = CurrentUserId
		};

		var nowUtc = DateTime.UtcNow;
		var tags = new List<string> { "tag1", "tag2" };

		var dateOnlyNow = DateOnly.FromDateTime(nowUtc);
		var dateOnlyEnd = DateOnly.FromDateTime(nowUtc.AddDays(3));

		CreateProjectResponse? project = null;
		try
		{
			project = await YandexTrackerClient.CreateProjectAsync(
				ProjectEntityType.Project,
				new CreateProjectRequest
				{
					Fields = new ProjectFieldsDto()
					{
						Summary = summary,
						Author = currentUserShortInfo.Id,
						Followers = [currentUserShortInfo.Id],
						Clients = [currentUserShortInfo.Id],
						Lead = currentUserShortInfo.Id,
						Description = "DESC",
						Tags = tags,
						Start = dateOnlyNow,
						End = dateOnlyEnd,
						TeamAccess = false,
						TeamUsers = [currentUserShortInfo.Id],
						EntityStatus = ProjectEntityStatus.InProgress
					}
				});


			await Task.Delay(1000); // Чтобы проекты точно создались в трекере

			var requestProject = new GetProjectsRequest()
			{
				Filter = new ProjectFieldsDto()
				{
					Summary = summary
				}
			};

			var assertedProjectShortId = project.ShortId;

			// Act
			var projects = await YandexTrackerClient.GetProjectsAsync(
				ProjectEntityType.Project,
				requestProject,
				returnedFields:
				ProjectFieldData.Summary | ProjectFieldData.Description | ProjectFieldData.Author
				| ProjectFieldData.Lead | ProjectFieldData.TeamUsers | ProjectFieldData.Clients
				| ProjectFieldData.Followers | ProjectFieldData.Start | ProjectFieldData.End
				| ProjectFieldData.Tags | ProjectFieldData.ParentEntity
				| ProjectFieldData.TeamAccess | ProjectFieldData.Quarter | ProjectFieldData.EntityStatus
				| ProjectFieldData.IssueQueues);

			// Assert
			Assert.IsNotNull(projects);

			var actualProject = projects.Values.FirstOrDefault(x => x.ShortId == assertedProjectShortId);
			Assert.IsNotNull(actualProject);
			Assert.AreEqual(summary, actualProject.Fields!.Summary);
			Assert.AreEqual(currentUserShortInfo.Id, actualProject.Fields!.Author!.Id);
			Assert.AreEqual(currentUserShortInfo.Id, actualProject.Fields!.Lead!.Id);
			Assert.AreEqual(1, actualProject.Fields!.Followers!.Count);
			Assert.AreEqual(currentUserShortInfo.Id, actualProject.Fields!.Followers!.First().Id);
			Assert.AreEqual(1, actualProject.Fields!.Clients!.Count);
			Assert.AreEqual(currentUserShortInfo.Id, actualProject.Fields!.Clients!.First().Id);
			Assert.AreEqual("DESC", actualProject.Fields!.Description);
			Assert.AreEqual(2, actualProject.Fields!.Tags!.Count);
			CollectionAssert.AreEquivalent(tags, actualProject.Fields!.Tags.ToArray());
			Assert.AreEqual(ProjectEntityType.Project, actualProject.EntityType);
			Assert.AreEqual(ProjectEntityStatus.InProgress, actualProject.Fields!.EntityStatus);
			Assert.AreEqual(dateOnlyNow, actualProject.Fields!.Start);
			Assert.AreEqual(dateOnlyEnd, actualProject.Fields!.End);
			Assert.AreEqual(currentUserShortInfo.Id, actualProject.CreatedBy.Id);
			Assert.IsNull(actualProject.Fields!.TeamAccess);
			Assert.AreEqual(1, actualProject.Fields!.TeamUsers!.Count);
			Assert.AreEqual(currentUserShortInfo.Id, actualProject.Fields!.TeamUsers!.First().Id);
			Assert.AreEqual(2, actualProject.Fields!.Quarter!.Count);
		}
		finally
		{
			if (project != null)
			{
				await SafeExecutor.ExecuteAsync(
					async() => await YandexTrackerClient.DeleteProjectAsync(
						ProjectEntityType.Project,
						project.ShortId,
						true));
			}
		}
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
		Assert.IsTrue(response.Values.Any());
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
		var response = await YandexTrackerClient.GetLocalQueueFieldsAsync(TestQueueKey);

		// Assert
		Assert.IsNotNull(response);
		Assert.IsTrue(response.Values.Any());
	}

	private string GetUniqueName()
	{
		return $"Testik{_index++}";
	}
}