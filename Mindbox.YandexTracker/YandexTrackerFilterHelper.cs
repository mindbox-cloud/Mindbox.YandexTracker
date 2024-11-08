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

/// <summary>
/// https://yandex.cloud/ru/docs/tracker/user/query-filter#query-functions
/// </summary>
public static class YandexTrackerFilterFunctions
{
	public const string Empty = "empty()";
	public const string NotEmpty = "notEmpty()";
	public const string Now = "now()";
	public const string Me = "me()";
	public const string Today = "today()";
	public const string Week = "week()";
	public const string Month = "month()";
	public const string Quarter = "quarter()";
	public const string Year = "year()";
	public const string Unresolved = "unresolved()";

	public static string Group(string groupName) => $"group(value: \"{groupName}\")";
}
