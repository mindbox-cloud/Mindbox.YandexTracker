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
