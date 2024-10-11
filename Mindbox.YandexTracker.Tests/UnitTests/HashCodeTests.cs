using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mindbox.YandexTracker.Template;

namespace Mindbox.YandexTracker.Tests;

[TestClass]
public class HashCodeTests
{
	[TestMethod]
	public void GetHashCode_TwoSimilarIssues_HashCodeShouldBeEqual()
	{
		var now = DateTime.Now;

		var issue1 = new Issue
		{
			Queue = "queue",
			Project = "project",
			Aliases = ["a1", "a2"],
			Assignee = new UserShortInfo
			{
				Id = "id"
			},
			Description = "desc",
			IsFavorite = false,
			CreatedBy = new UserShortInfo
			{
				Id = "id2"
			},
			CustomFields = new Dictionary<string, object?>
			{
				["field1"] = 5,
				["field2"] = "field",
				["field3"] = new UserShortInfo { Id = "id3" },
			}!,
			CreatedAt = now,
			Key = "key",
			Priority = Priority.Critical,
			Sprints = ["sprint"],
			Type = new IssueType
			{
				Key = "key",
				Name = "name"
			}
		};

		var issue2 = new Issue
		{
			Queue = "queue",
			Project = "project",
			Aliases = ["a1", "a2"],
			Assignee = new UserShortInfo
			{
				Id = "id"
			},
			Description = "desc",
			IsFavorite = false,
			CreatedBy = new UserShortInfo
			{
				Id = "id2"
			},
			CustomFields = new Dictionary<string, object?>
			{
				["field1"] = 5,
				["field2"] = "field",
				["field3"] = new UserShortInfo { Id = "id3" },
			}!,
			CreatedAt = now,
			Key = "key",
			Priority = Priority.Critical,
			Sprints = ["sprint"],
			Type = new IssueType
			{
				Key = "key",
				Name = "name"
			}
		};

		Assert.AreEqual(issue1.GetHashCode(), issue2.GetHashCode());
	}

	[TestMethod]
	public void GetHashCode_TwoSimilarIssueFilters_HashCodeShouldBeEqual()
	{
		var now = DateTime.Now;

		var issueFilter1 = new IssuesFilter
		{
			Aliases = ["a1", "a2", "a3"],
			Assignee = "me",
			Description = "desc",
			LastCommentUpdatedAt = now,
			IsFavorite = true,
			CreatedBy = "on",
			Followers = ["follower1", "follower2"],
			Queue = "que",
			Project = "da",
			Summary = "sumik",
			Priority = Priority.Minor,
			Sprints = ["sp1"],
			Parent = "par1",
			Status = new IssueStatus
			{
				Key = "key",
				Name = "name"
			},
			CreatedAt = now,
			Votes = 5,
			Type = new IssueType
			{
				Key = "type",
				Name = "net"
			},
			PreviousStatus = new IssueStatus
			{
				Key = "da",
				Name = "lastName"
			},
			UpdatedAt = now.AddHours(3),
			UpdatedBy = "e"
		};

		var issueFilter2 = new IssuesFilter
		{
			Aliases = ["a1", "a2", "a3"],
			Assignee = "me",
			Description = "desc",
			LastCommentUpdatedAt = now,
			IsFavorite = true,
			CreatedBy = "on",
			Followers = ["follower1", "follower2"],
			Queue = "que",
			Project = "da",
			Summary = "sumik",
			Priority = Priority.Minor,
			Sprints = ["sp1"],
			Parent = "par1",
			Status = new IssueStatus
			{
				Key = "key",
				Name = "name"
			},
			CreatedAt = now,
			Votes = 5,
			Type = new IssueType
			{
				Key = "type",
				Name = "net"
			},
			PreviousStatus = new IssueStatus
			{
				Key = "da",
				Name = "lastName"
			},
			UpdatedAt = now.AddHours(3),
			UpdatedBy = "e"
		};

		Assert.AreEqual(issueFilter1.GetHashCode(), issueFilter2.GetHashCode());
	}

	[TestMethod]
	public void GetHashCode_TwoSimilarProjects_HashCodeShouldBeEqual()
	{
		var future = DateTime.Now.AddDays(1);

		var project1 = new Project
		{
			Author = new UserShortInfo
			{
				Id = "id"
			},
			Id = "5",
			ChecklistIds = ["ch1", "ch2"],
			Clients = [
				new()
				{
					Id = "id2"
				}
			],
			CreatedAt = future,
			CreatedBy = new UserShortInfo
			{
				Id = "w"
			},
			End = future.AddDays(-3),
			Lead = new UserShortInfo
			{
				Id = "id"
			},
			Quarter = ["q1", "q2"],
			ParentId = 3,
			Description = "project for project",
			Start = future.AddDays(2),
			Status = ProjectEntityStatus.Draft,
			Summary = "summary",
			ShortId = 2
		};

		var project2 = new Project
		{
			Author = new UserShortInfo
			{
				Id = "id"
			},
			Id = "5",
			ChecklistIds = ["ch1", "ch2"],
			Clients = [
				new()
				{
					Id = "id2"
				}
			],
			CreatedAt = future,
			CreatedBy = new UserShortInfo
			{
				Id = "w"
			},
			End = future.AddDays(-3),
			Lead = new UserShortInfo
			{
				Id = "id"
			},
			Quarter = ["q1", "q2"],
			ParentId = 3,
			Description = "project for project",
			Start = future.AddDays(2),
			Status = ProjectEntityStatus.Draft,
			Summary = "summary",
			ShortId = 2
		};

		Assert.AreEqual(project1.GetHashCode(), project2.GetHashCode());
	}

	[TestMethod]
	public void GetCollectionHashCode_TwoSimilarCollections_HashCodeShouldBeEqual()
	{
		var list1 = new List<string>
		{
			"Mindbox",
			"Yandex",
			"Ozon"
		};

		var list2 = new List<string>
		{
			"Mindbox",
			"Yandex",
			"Ozon"
		};

		Assert.AreEqual(list1.GetCollectionHashCode(), list2.GetCollectionHashCode());
	}
}
