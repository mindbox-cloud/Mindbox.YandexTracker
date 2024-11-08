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

/// <summary>
/// Дополнительные поля, которые будут включены в ответ. Возможен множественный выбор.
/// </summary>
[Flags]
public enum QueueExpandData
{
	None = 0,
	Projects = 0x0001,
	Components = 0x0002,
	Versions = 0x0004,
	Types = 0x0008,
	Team = 0x0010,
	Workflows = 0x0020,
	Fields = 0x0040,
	IssueTypesConfig = 0x0080,
	All = Projects | Components | Versions | Types | Team | Workflows | Fields | IssueTypesConfig
}