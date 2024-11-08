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

public sealed record CreateProjectRequest
{
	public required ProjectFieldsDto Fields { get; init; }
}

public sealed record ProjectFieldsDto
{
	public required string Summary { get; init; }

	public bool? TeamAccess { get; init; }

	public string? Description { get; init; }

	public string? Author { get; init; }

	public string? Lead { get; init; }

	public IReadOnlyCollection<string>? TeamUsers { get; init; }

	public IReadOnlyCollection<string>? Clients { get; init; }

	public IReadOnlyCollection<string>? Followers { get; init; }

	public DateOnly? Start { get; init; }

	public DateOnly? End { get; init; }

	public IReadOnlyCollection<string>? Tags { get; init; }

	public int? ParentEntity { get; init; }

	public ProjectEntityStatus? EntityStatus { get; init; }
}