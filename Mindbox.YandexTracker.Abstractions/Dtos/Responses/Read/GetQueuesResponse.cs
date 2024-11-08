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
using System.Collections.Immutable;

namespace Mindbox.YandexTracker;

public sealed record GetQueuesResponse
{
	public required long Id { get; init; }

	public required int Version { get; init; }

	public required string Key { get; init; }

	public required string Name { get; init; }

	public string? Description { get; init; }

	public required UserShortInfoDto Lead { get; init; }

	public bool AssignAuto { get; init; }

	public required FieldInfo DefaultType { get; init; }

	public required FieldInfo DefaultPriority { get; init; }

	public IReadOnlyCollection<UserShortInfoDto> TeamUsers { get; init; } = [];

	public IReadOnlyCollection<FieldInfo> IssueTypes { get; init; } = [];

	public IReadOnlyCollection<FieldInfo> Versions { get; init; } = [];

	public IReadOnlyDictionary<string, IReadOnlyCollection<FieldInfo>> Workflows { get; init; }
		= ImmutableDictionary<string, IReadOnlyCollection<FieldInfo>>.Empty;

	public IReadOnlyCollection<IssueTypeConfigDto> IssueTypesConfig { get; init; } = [];

	public bool DenyVoting { get; init; }
}