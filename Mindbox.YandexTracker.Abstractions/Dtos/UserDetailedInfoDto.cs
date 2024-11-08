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

namespace Mindbox.YandexTracker;

public sealed record UserDetailedInfoDto
{
	public required long Uid { get; init; }

	public required string Login { get; init; }

	public required long TrackerUid { get; init; }

	public long? PassportUid { get; init; }

	public string? CloudUid { get; init; }

	public required string FirstName { get; init; }

	public required string LastName { get; init; }

	public required string Display { get; init; }

	public required string Email { get; init; }

	public bool External { get; init; }

	public bool HasLicense { get; init; }

	public bool Dismissed { get; init; }

	public bool UseNewFilters { get; init; }

	public bool DisableNotifications { get; init; }

	public DateTime FirstLoginDate { get; init; }

	public DateTime LastLoginDate { get; init; }

	public bool WelcomeMailSent { get; init; }
}