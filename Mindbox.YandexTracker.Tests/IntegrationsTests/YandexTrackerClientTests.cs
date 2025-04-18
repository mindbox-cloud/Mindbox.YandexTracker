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
	public async Task CreateIssueAsync_WithCustomFields_ShouldCreatedIssueWithCustomFields()
	{
		var firstCategory = (await YandexTrackerClient.GetFieldCategoriesAsync()).Values[0];

		var customStringField = await YandexTrackerClient.CreateLocalFieldInQueueAsync(
			TestQueueKey,
			new CreateQueueLocalFieldRequest
		{
			Id = "customStringField",
			Category = firstCategory.Id,
			Name = new QueueLocalFieldName
			{
				En = "customStringField",
				Ru = "customStringField"
			},
			Type = QueueLocalFieldType.StringFieldType
		});
		var customFloatField = await YandexTrackerClient.CreateLocalFieldInQueueAsync(
			TestQueueKey,
			new CreateQueueLocalFieldRequest
		{
			Id = "customFloatField",
			Category = firstCategory.Id,
			Name = new QueueLocalFieldName
			{
				En = "customFloatField",
				Ru = "customFloatField"
			},
			Type = QueueLocalFieldType.FloatFieldType
		});
		var customIntField = await YandexTrackerClient.CreateLocalFieldInQueueAsync(
			TestQueueKey,
			new CreateQueueLocalFieldRequest
		{
			Id = "customIntField",
			Category = firstCategory.Id,
			Name = new QueueLocalFieldName
			{
				En = "customIntField",
				Ru = "customIntField"
			},
			Type = QueueLocalFieldType.IntegerFieldType
		});
		var customDateTimeField = await YandexTrackerClient.CreateLocalFieldInQueueAsync(
			TestQueueKey,
			new CreateQueueLocalFieldRequest
		{
			Id = "customDateTimeField",
			Category = firstCategory.Id,
			Name = new QueueLocalFieldName
			{
				En = "customDateField",
				Ru = "customDateField"
			},
			Type = QueueLocalFieldType.DateTimeFieldType
		});

		var expectedCustomSelect = "option1";
		var customSelectField = await YandexTrackerClient.CreateLocalFieldInQueueAsync(
			TestQueueKey,
			new CreateQueueLocalFieldRequest
		{
			Id = "customSelectField",
			Category = firstCategory.Id,
			Name = new QueueLocalFieldName
			{
				En = "customSelectField",
				Ru = "customSelectField"
			},
			Type = QueueLocalFieldType.StringFieldType,
			OptionsProvider = new OptionsProviderInfoDto
			{
				Type = "FixedListOptionsProvider",
				Values =
				[
					expectedCustomSelect,
					"option 2"
				]
			}
		});
		var customMultiSelectField = await YandexTrackerClient.CreateLocalFieldInQueueAsync(
			TestQueueKey,
			new CreateQueueLocalFieldRequest
		{
			Id = "customMultiSelectField",
			Category = firstCategory.Id,
			Name = new QueueLocalFieldName
			{
				En = "customMultiSelectField",
				Ru = "customMultiSelectField"
			},
			Type = QueueLocalFieldType.StringFieldType,
			Container = true
		});

		await Task.Delay(TimeSpan.FromSeconds(2)); // Чтобы поле точно создалось в трекере

		var issue = new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName(),
			Project = TestProjectShortId
		};

		var expectedCustomString = "field1";
		var expectedCustomFloat = 10.0;
		var expectedCustomInt = 15;
		var expectedCustomDateTime = new DateTime(2021, 10, 10, 10, 10, 0);
		var expectedCustomMultiSelect = new List<string> {
			"option1",
			"option2"
		};

		issue.SetCustomField(customStringField.Id, expectedCustomString);
		issue.SetCustomField(customFloatField.Id, expectedCustomFloat);
		issue.SetCustomField(customIntField.Id, expectedCustomInt);
		issue.SetCustomField(customDateTimeField.Id, expectedCustomDateTime);
		issue.SetCustomField(customSelectField.Id, expectedCustomSelect);
		issue.SetCustomField(customMultiSelectField.Id, expectedCustomMultiSelect);

		var createdIssue = await YandexTrackerClient.CreateIssueAsync(issue);

		var response = await YandexTrackerClient.GetIssueAsync(createdIssue.Key);

		Assert.IsNotNull(response);
		Assert.AreEqual(expectedCustomString, response.GetCustomField<string>(customStringField.Id));
		Assert.AreEqual(expectedCustomFloat, response.GetCustomField<double>(customFloatField.Id));
		Assert.AreEqual(expectedCustomInt, response.GetCustomField<int>(customIntField.Id));
		Assert.AreEqual(expectedCustomDateTime.Ticks, response.GetCustomField<DateTime>(customDateTimeField.Id).Ticks);
		Assert.AreEqual(expectedCustomSelect, response.GetCustomField<string>(customSelectField.Id));
		var actualCustomMultiSelectValue = response.GetCustomField<List<string>>(customMultiSelectField.Id);
		Assert.AreEqual(actualCustomMultiSelectValue.Count, expectedCustomMultiSelect.Count);
		Assert.AreEqual(actualCustomMultiSelectValue.Count, 2);
		Assert.AreEqual(actualCustomMultiSelectValue[0], expectedCustomMultiSelect[0]);
		Assert.AreEqual(actualCustomMultiSelectValue[1], expectedCustomMultiSelect[1]);
	}

	[TestMethod]
	public async Task BulkUpdateIssueAsync_WithCustomFields_ShouldUpdateIssueWithCustomFields()
	{
		// Arrange
		var issue = new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName(),
		};
		var createdIssue = await YandexTrackerClient.CreateIssueAsync(issue);

		// Act

		var updatesIssuesValues = new IssuesBulkUpdateValues();
		updatesIssuesValues.SetValue("project", TestProjectShortId);
		var updateIssueRequest = new IssuesBulkUpdateRequest
		{
			Issues = [createdIssue.Key],
			Values = updatesIssuesValues
		};

		var bulkUpdateResponse = await YandexTrackerClient.BulkUpdateIssuesAsync(updateIssueRequest);

		bool isBulkOperationRunning;
		var failCheckAfter = DateTime.UtcNow.AddMinutes(1);
		do
		{
			var bulkChangeStatus = await YandexTrackerClient.GetBulkChangeStatusAsync(bulkUpdateResponse.Id);

			isBulkOperationRunning = bulkChangeStatus.Status is "RUNNING" or "CREATED";

			Assert.IsTrue(DateTime.UtcNow <= failCheckAfter, "Bulk operation is running too long");

		} while (isBulkOperationRunning);

		// Assert
		var updatedIssue = await YandexTrackerClient.GetIssueAsync(createdIssue.Key);

		Assert.IsNotNull(updatedIssue);
		Assert.AreEqual(createdIssue.Description, updatedIssue.Description);
		Assert.AreEqual(TestProjectShortId.ToString(CultureInfo.InvariantCulture), updatedIssue.Project?.Id);
	}

	[TestMethod]
	public async Task UpdateIssuesAsync_WithCustomFields_ShouldUpdateIssueWithCustomFields()
	{
		// Arrange
		var firstCategory = (await YandexTrackerClient.GetFieldCategoriesAsync()).Values[0];

		var customStringField = await YandexTrackerClient.CreateLocalFieldInQueueAsync(
			TestQueueKey,
			new CreateQueueLocalFieldRequest
		{
			Id = "customUpdatedStringField",
			Category = firstCategory.Id,
			Name = new QueueLocalFieldName
			{
				En = "customStringField",
				Ru = "customStringField"
			},
			Type = QueueLocalFieldType.StringFieldType
		});
		var customString2Field = await YandexTrackerClient.CreateLocalFieldInQueueAsync(
			TestQueueKey,
			new CreateQueueLocalFieldRequest
		{
			Id = "customUpdatedString2Field",
			Category = firstCategory.Id,
			Name = new QueueLocalFieldName
			{
				En = "customString2Field",
				Ru = "customString2Field"
			},
			Type = QueueLocalFieldType.StringFieldType
		});

		await Task.Delay(TimeSpan.FromSeconds(2)); // Чтобы поле точно создалось в трекере

		var issue = new CreateIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName(),
			Description = "Old descritpion",
		};
		var expectedCustom2String = "field2_unchanged";

		issue.SetCustomField(customStringField.Id, "field1_unchanged");
		issue.SetCustomField(customString2Field.Id, expectedCustom2String);

		var createdIssue = await YandexTrackerClient.CreateIssueAsync(issue);

		// Act
		var updateIssueRequest = new UpdateIssueRequest
		{
			Summary = "New summary",
			Project = TestProjectShortId
		};
		var expectedCustomString = "field1_changed";
		updateIssueRequest.SetCustomField(customStringField.Id, expectedCustomString);


		await YandexTrackerClient.UpdateIssueAsync(
			createdIssue.Key,
			updateIssueRequest);

		// Assert
		var updatedIssue = await YandexTrackerClient.GetIssueAsync(createdIssue.Key);

		Assert.IsNotNull(updatedIssue);
		Assert.AreEqual(expectedCustomString, updatedIssue.GetCustomField<string>(customStringField.Id));
		Assert.AreEqual(expectedCustom2String, updatedIssue.GetCustomField<string>(customString2Field.Id));
		Assert.AreEqual(updateIssueRequest.Summary, updatedIssue.Summary);
		Assert.AreEqual(createdIssue.Description, updatedIssue.Description);
		Assert.AreEqual(updateIssueRequest.Project?.ToString(CultureInfo.InvariantCulture), updatedIssue.Project?.Id);
	}

	[TestMethod]
	[Ignore("Методы должны вызываться только администратором Яндекс.Трекера. В CI/CD используется обычный пользователь")]
	public async Task FullImportIssueFlow()
	{
		var firstCategory = (await YandexTrackerClient.GetFieldCategoriesAsync()).Values[0];
		var resolutions = await YandexTrackerClient.GetResolutionsAsync();
		var expectedResolution = resolutions.Values[0];

		var customField = await YandexTrackerClient.CreateLocalFieldInQueueAsync(TestQueueKey, new CreateQueueLocalFieldRequest
		{
			Id = "customFieldsForImport",
			Category = firstCategory.Id,
			Name = new QueueLocalFieldName
			{
				En = "customFieldsForImport",
				Ru = "Кастомное поле для импорта"
			},
			Type = QueueLocalFieldType.StringFieldType
		});

		await Task.Delay(TimeSpan.FromSeconds(2)); // Чтобы поле точно создалось в трекере

		// импортируем задачу
		var issueCreatedAt = new DateTime(2021, 10, 10, 10, 10, 10);
		var issueResolvedAt = issueCreatedAt.AddMinutes(5);
		var issueUpdatedAt = issueCreatedAt.AddMinutes(10);

		var importIssueRequest = new ImportIssueRequest
		{
			Queue = TestQueueKey,
			Summary = GetUniqueName(),
			Project = TestProjectShortId,
			StoryPoints = 1.5,

			CreatedBy = CurrentUserId,
			CreatedAt = issueCreatedAt,

			UpdatedAt = issueUpdatedAt,
			UpdatedBy = CurrentUserId,

			ResolvedAt = issueResolvedAt,
			ResolvedBy = CurrentUserLogin,
			Resolution = expectedResolution.Id
		};
		importIssueRequest.SetCustomField<string>(customField.Id, "field1");

		var importedIssue = await YandexTrackerClient.ImportIssueAsync(importIssueRequest);

		var createdAttachmentIds = new List<string>();
		try
		{
			// импортируем комментарий
			var commentCreatedAt = new DateTime(2021, 10, 10, 10, 10, 12);
			var commentUpdatedAt = issueCreatedAt.AddMinutes(2);
			var importCommentRequest = new ImportCommentRequest
			{
				CreatedAt = commentCreatedAt,
				CreatedBy = CurrentUserId,
				UpdatedAt = commentUpdatedAt,
				UpdatedBy = CurrentUserId,
				Text = "Comment text"
			};
			var importCommentResponse = await YandexTrackerClient.ImportCommentAsync(importedIssue.Key, importCommentRequest);

			// загрузим вложения

			await using var imageFile = File.OpenRead(Path.Combine("TestFiles", "pepe.png"));
			await using var txtFile = File.OpenRead(Path.Combine("TestFiles", "importantInformation.txt"));

			var issueAttachmentResponse = await YandexTrackerClient.ImportAttachmentToIssueAsync(
				importedIssue.Key,
				imageFile,
				imageFile.Name,
				issueCreatedAt,
				CurrentUserId);
			createdAttachmentIds.Add(issueAttachmentResponse.Id);
			var commentAttachmentResponse = await YandexTrackerClient.ImportAttachmentToIssueCommentAsync(
				importedIssue.Key,
				importCommentResponse.Id.ToString(CultureInfo.InvariantCulture),
				txtFile,
				txtFile.Name,
				commentCreatedAt,
				CurrentUserId);
			createdAttachmentIds.Add(commentAttachmentResponse.Id);

			await Task.Delay(TimeSpan.FromSeconds(5)); // Чтобы Трекер все успел обработать

			// получим issue с комментами и проверим
			var issue = await YandexTrackerClient.GetIssueAsync(importedIssue.Key, IssueExpandData.Attachments);
			var comments = await YandexTrackerClient.GetCommentsAsync(importedIssue.Key, CommentExpandData.All);

			Assert.IsNotNull(issue);
			Assert.AreEqual(issueCreatedAt, issue.CreatedAt);
			Assert.IsNotNull(issue.CreatedBy);
			Assert.AreEqual(CurrentUserId, issue.CreatedBy.Id);
			Assert.AreEqual(issueUpdatedAt, issue.UpdatedAt);
			Assert.IsNotNull(issue.UpdatedBy);
			Assert.AreEqual(CurrentUserId, issue.UpdatedBy.Id);
			Assert.AreEqual(issueResolvedAt, issue.ResolvedAt);
			Assert.IsNotNull(issue.ResolvedBy);
			Assert.AreEqual(CurrentUserId, issue.ResolvedBy.Id);
			Assert.IsNotNull(issue.Resolution);
			Assert.AreEqual(expectedResolution.Id.ToString(CultureInfo.InvariantCulture), issue.Resolution!.Id);
			Assert.IsNull(issue.Author);
			Assert.IsNull(issue.Parent);
			Assert.IsTrue(issue.Sprint.Count == 0);
			Assert.AreEqual(importIssueRequest.StoryPoints, issue.StoryPoints);

			Assert.AreEqual("field1", issue.GetCustomField<string>(customField.Id));

			Assert.IsTrue(issue.Attachments.Any(x => x.Id == issueAttachmentResponse.Id));

			var actualComment = comments.Values.SingleOrDefault();
			Assert.IsNotNull(actualComment);
			Assert.AreEqual(importCommentResponse.Id, actualComment.Id);
			Assert.AreEqual(commentCreatedAt, actualComment.CreatedAt);
			Assert.AreEqual(CurrentUserId, actualComment.CreatedBy.Id);
			Assert.AreEqual(commentUpdatedAt, actualComment.UpdatedAt);
			Assert.AreEqual(CurrentUserId, actualComment.UpdatedBy.Id);
			Assert.AreEqual(importCommentRequest.Text, actualComment.Text);

			Assert.IsTrue(actualComment.Attachments.Any(x => x.Id == commentAttachmentResponse.Id));
		}
		finally
		{
			foreach (var attachmentId in createdAttachmentIds)
			{
				await SafeExecutor.ExecuteAsync(async () =>
					await YandexTrackerClient.DeleteAttachmentAsync(importedIssue.Key, attachmentId));
			}
		}
	}

	[TestMethod]
	public async Task CreateLocalFieldInQueueAsync_ValidRequest_ShouldCreatedLocalField()
	{
		var firstCategory = (await YandexTrackerClient.GetFieldCategoriesAsync()).Values[0];

		var localField = new CreateQueueLocalFieldRequest
		{
			Id = "some_Id3",
			Name = new QueueLocalFieldName
			{
				En = "английское название",
				Ru = "русское название"
			},
			Category = firstCategory.Id,
			Type = QueueLocalFieldType.DateFieldType
		};

		var response = await YandexTrackerClient.CreateLocalFieldInQueueAsync(TestQueueKey, localField);

		Assert.IsNotNull(response);
		Assert.IsTrue(response.Id.EndsWith(localField.Id, StringComparison.InvariantCulture));
	}

	[TestMethod]
	public async Task UpdateLocalFieldInQueueAsync_ValidRequest_UpdateCreatedLocalField()
	{
		const string optionsProviderType = "FixedListOptionsProvider";

		var firstCategory = (await YandexTrackerClient.GetFieldCategoriesAsync()).Values[0];

		var optionValues = new List<string>
		{
			"1",
			"2"
		};
		var localField = new CreateQueueLocalFieldRequest
		{
			Id = "some_Id4",
			Name = new QueueLocalFieldName
			{
				En = "английское название",
				Ru = "русское название"
			},
			Category = firstCategory.Id,
			Type = QueueLocalFieldType.StringFieldType,
			OptionsProvider = new OptionsProviderInfoDto
			{

				Type = optionsProviderType,
				Values = optionValues
			}
		};

		var createResponse = await YandexTrackerClient.CreateLocalFieldInQueueAsync(TestQueueKey, localField);
		Assert.IsNotNull(createResponse);

		await Task.Delay(1000); // Чтобы поле точно создалось в трекере

		optionValues.Add("3");

		var updateRequest = new UpdateQueueLocalFieldRequest
		{
			OptionsProvider = new OptionsProviderInfoDto
			{
				Type = optionsProviderType,
				Values = optionValues
			}
		};
		var updateResponse = await YandexTrackerClient.UpdateLocalFieldInQueueAsync(
			TestQueueKey,
			createResponse.Key,
			updateRequest);

		Assert.IsNotNull(updateResponse);
		Assert.IsTrue(updateResponse.Id.EndsWith(localField.Id, StringComparison.InvariantCulture));
		Assert.IsNotNull(updateResponse.OptionsProvider);
		Assert.AreEqual(optionValues.Count, updateResponse.OptionsProvider.Values.Count);

		var expectedOptionValues = optionValues.OrderBy(x => x).ToList();
		var actualOptionValues = updateResponse.OptionsProvider.Values.OrderBy(x => x).ToList();

		for (var i = 0; i < optionValues.Count; i++)
		{
			Assert.AreEqual(expectedOptionValues[i], actualOptionValues[i]);
		}
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
			new CreateComponentRequest
			{
				Name = GetUniqueName(),
				Queue = TestQueueKey
			});
		var component2 = await YandexTrackerClient.CreateComponentAsync(
			new CreateComponentRequest
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
	public async Task AttachmentsApiTest()
	{
		CreateAttachmentResponse? imageAttachment = null;
		CreateAttachmentResponse? textAttachment = null;
		string issueKey = null!;
		try
		{
			var issue = await YandexTrackerClient.CreateIssueAsync(new CreateIssueRequest
			{
				Queue = TestQueueKey,
				Summary = GetUniqueName()
			});
			issueKey = issue.Key;

			await using var imageFile = File.OpenRead(Path.Combine("TestFiles", "pepe.png"));
			await using var txtFile = File.OpenRead(Path.Combine("TestFiles", "importantInformation.txt"));

			imageAttachment = await YandexTrackerClient.CreateAttachmentAsync(
				issue.Key,
				imageFile);

			textAttachment = await YandexTrackerClient.CreateAttachmentAsync(
				issue.Key,
				txtFile);

			await Task.Delay(3000); // Чтобы вложения точно создались в трекере

			CreateAttachmentResponse[] expectedAttachments = [imageAttachment, textAttachment];

			var attachments = await YandexTrackerClient.GetAttachmentsAsync(issue.Key);

			Assert.IsNotNull(attachments);
			Assert.AreEqual(expectedAttachments.Length, attachments.Values.Count);
		}
		finally
		{
			if (imageAttachment != null)
			{
				await SafeExecutor.ExecuteAsync(async () =>
					await YandexTrackerClient.DeleteAttachmentAsync(issueKey, imageAttachment.Id));
			}
			if (textAttachment != null)
			{
				await SafeExecutor.ExecuteAsync(async () =>
					await YandexTrackerClient.DeleteAttachmentAsync(issueKey, textAttachment.Id));
			}
		}
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

		var currentUserShortInfo = new UserShortInfoDto
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
					Fields = new ProjectFieldsDto
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
						TeamAccess = true,
						TeamUsers = [currentUserShortInfo.Id],
						EntityStatus = ProjectEntityStatus.InProgress
					}
				});


			await Task.Delay(1000); // Чтобы проекты точно создались в трекере

			var requestProject = new GetProjectsRequest
			{
				Filter = new ProjectFieldsDto
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