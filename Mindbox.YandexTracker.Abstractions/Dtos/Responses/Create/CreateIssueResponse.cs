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

namespace Mindbox.YandexTracker;

/// <summary>
/// Ответ на запрос создания задачи <see cref="CreateIssueRequest"/>.
/// </summary>
public record CreateIssueResponse : CustomFieldsResponse
{
	public required string Id { get; init; }

	public required string Key { get; init; }

	public required FieldInfo Queue { get; init; }

	public required int Version { get; init; }

	public required string Summary { get; init; }

	public FieldInfo? Parent { get; init; }

	public DateTime CreatedAt { get; init; }

	public required UserShortInfoDto CreatedBy { get; init; }

	public DateTime? UpdatedAt { get; init; }

	public UserShortInfoDto? UpdatedBy { get; init; }

	public required FieldInfo Type { get; init; }

	public required FieldInfo Status { get; init; }

	public DateTime? StatusStartTime { get; init; }

	public FieldInfo? PreviousStatus { get; init; }

	public string? Description { get; init; }

	public UserShortInfoDto? Author { get; init; }

	public UserShortInfoDto? Assignee { get; init; }

	public DateOnly? Start { get; init; }

	public DateOnly? End { get; init; }

	public DateOnly? DueDate { get; init; }

	public double? StoryPoints { get; init; }

	public FieldInfo? Project { get; init; }

	public IReadOnlyCollection<FieldInfo> Sprint { get; init; } = [];

	public IReadOnlyCollection<UserShortInfoDto> Followers { get; init; } = [];

	public IReadOnlyCollection<string> Aliases { get; init; } = [];

	public required FieldInfo Priority { get; init; }

	public int Votes { get; init; }

	public bool Favorite { get; init; }

	public IReadOnlyCollection<UserShortInfoDto> Access { get; init; } = [];

	public int? PossibleSpam { get; init; }

	public UserShortInfoDto? QaEngineer { get; init; }

	public DateTime? LastCommentUpdatedAt { get; init; }
}
