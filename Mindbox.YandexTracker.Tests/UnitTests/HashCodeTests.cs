using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mindbox.YandexTracker.Tests;

[TestClass]
public class HashCodeTests
{
	[TestMethod]
	public void GetHashCode_TwoSimilarIssueFilters_HashCodeShouldBeEqual()
	{
		var now = DateTime.Now;

		var issueFilter1 = new IssuesFilterDto
		{
			Assignee = ["me"],
			Description = ["desc"],
			LastCommentUpdatedAt = now,
			CreatedBy = ["on"],
			Followers = ["follower1", "follower2"],
			Queue = ["que"],
			Project = ["da"],
			Summary = ["sumik"],
			Priority = [Priority.Minor],
			Sprint = ["sp1"],
			Parent = ["par1"],
			Status = ["key"],
			CreatedAt = now,
			Votes = 5,
			Type = ["type"],
			PreviousStatus = ["da"],
			UpdatedAt = now.AddHours(3),
			UpdatedBy = ["e"]
		};

		var issueFilter2 = new IssuesFilterDto
		{
			Assignee = ["me"],
			Description = ["desc"],
			LastCommentUpdatedAt = now,
			CreatedBy = ["on"],
			Followers = ["follower1", "follower2"],
			Queue = ["que"],
			Project = ["da"],
			Summary = ["sumik"],
			Priority = [Priority.Minor],
			Sprint = ["sp1"],
			Parent = ["par1"],
			Status = ["key"],
			CreatedAt = now,
			Votes = 5,
			Type = ["type"],
			PreviousStatus = ["da"],
			UpdatedAt = now.AddHours(3),
			UpdatedBy = ["e"]
		};

		Assert.AreEqual(issueFilter1.GetHashCode(), issueFilter2.GetHashCode());
	}
}
