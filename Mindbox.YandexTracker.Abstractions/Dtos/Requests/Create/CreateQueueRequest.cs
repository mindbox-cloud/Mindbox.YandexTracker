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

using System.Collections.Generic;

namespace Mindbox.YandexTracker;

public sealed record CreateQueueRequest
{
	public required string Key { get; init; }

	public required string Name { get; init; }

	public required string Lead { get; init; }

	public required string DefaultType { get; init; }

	public Priority DefaultPriority { get; init; }

	public IReadOnlyCollection<CreateIssueTypeConfigDto>? IssueTypesConfig { get; init; }
}

public sealed record CreateIssueTypeConfigDto
{
	public required string IssueType { get; init; }

	public required string Workflow { get; init; }

	public IReadOnlyCollection<string>? Resolutions { get; init; }
}