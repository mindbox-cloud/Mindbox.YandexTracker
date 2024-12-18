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

namespace Mindbox.YandexTracker;

public sealed record UpdateQueueLocalFieldRequest
{
	public QueueLocalFieldName? Name { get; init; }

	public string? Category { get; init; }

	public OptionsProviderInfoDto? OptionsProvider { get; init; }

	public int? Order { get; init; }

	public string? Description { get; init; }

	public bool? Readonly { get; init; }

	public bool? Visible { get; init; }

	public bool? Hidden { get; init; }

	public bool? Container { get; init; }
}