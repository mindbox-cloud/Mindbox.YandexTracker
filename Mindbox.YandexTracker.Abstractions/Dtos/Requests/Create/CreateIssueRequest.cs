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

public sealed record CreateIssueRequest : CustomFieldsRequest
{
	public required string Summary { get; init; }

	public required string Queue { get; init; }

	public IReadOnlyCollection<string>? Followers { get; init; }

	public string? Type { get; init; }

	public string? Description { get; init; }

	public string? Parent { get; init; }

	public DateOnly? Start { get; init; }
	public DateOnly? End { get; init; }
	public DateOnly? DueDate { get; init; }

	public string? Author { get; init; }

	public string? QaEngineer { get; init; }

	public IReadOnlyCollection<string>? Access { get; init; }

	public int? PossibleSpam { get; init; }

	public string? Unique { get; init; }

	public IReadOnlyCollection<string>? AttachmentIds { get; init; }

	public IReadOnlyCollection<string>? Sprint { get; init; }

	public Priority? Priority { get; init; }

	public string? Assignee { get; init; }

	public IReadOnlyCollection<string>? Tags { get; init; }
}
