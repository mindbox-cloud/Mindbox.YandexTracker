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

public sealed record GetCommentsResponse
{
	public int Id { get; init; }

	public required string LongId { get; init; }

	public int Version { get; init; }

	public string? Text { get; init; }

	public string? TextHtml { get; init; }

	public IReadOnlyCollection<FieldInfo> Attachments { get; init; } = [];

	public DateTime CreatedAt { get; init; }

	public DateTime UpdatedAt { get; init; }

	public required UserShortInfoDto CreatedBy { get; init; }

	public required UserShortInfoDto UpdatedBy { get; init; }

	public CommentType Type { get; init; }

	public CommentTransportType Transport { get; init; }
}
