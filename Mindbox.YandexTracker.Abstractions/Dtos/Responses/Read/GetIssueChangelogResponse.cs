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
using System.Text.Json;

namespace Mindbox.YandexTracker;

public sealed record GetIssueChangelogResponse
{
	public required string Id { get; init; }
	public required FieldInfo Issue { get; init; }
	public required DateTime UpdatedAt { get; init; }
	public UserShortInfoDto? UpdatedBy { get; init; }
	public string? Transport { get; init; }
	public IssueChangeType Type { get; init; }
	public IReadOnlyCollection<FieldChangeDto> Fields { get; init; } = [];
	public CommentsDto? Comments { get; init; }
	public IReadOnlyCollection<ExecutedTriggersDto> ExecutedTriggers { get; init; } = [];
}

public sealed record CommentsDto
{
	public IReadOnlyCollection<FieldInfo> Added { get; init; } = [];
}

public sealed record FieldChangeDto
{
	public required FieldInfo Field { get; init; }
	public JsonElement? From { get; init; }
	public JsonElement? To { get; init; }
}

public sealed record ExecutedTriggersDto
{
	public bool Success { get; init; }
	public string? Message { get; init; }
	public required FieldInfo Trigger { get; init; }
}