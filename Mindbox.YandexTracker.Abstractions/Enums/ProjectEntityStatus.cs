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

using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Статус проекта/портфеля
/// </summary>
public enum ProjectEntityStatus
{
	/// <summary>
	/// Черновик
	/// </summary>
	[EnumMember(Value = "draft")]
	Draft,

	/// <summary>
	/// В работе
	/// </summary>
	[EnumMember(Value = "in_progress")]
	InProgress,

	/// <summary>
	/// Новый
	/// </summary>
	[EnumMember(Value = "launched")]
	Launched,

	/// <summary>
	/// Отложен
	/// </summary>
	[EnumMember(Value = "postponed")]
	Postponed,

	/// <summary>
	/// Есть риски
	/// </summary>
	[EnumMember(Value = "at_risk")]
	AtRisk,

	/// <summary>
	/// Заблокирован
	/// </summary>
	[EnumMember(Value = "blocked")]
	Blocked,

	/// <summary>
	/// По плану
	/// </summary>
	[EnumMember(Value = "according_to_plan")]
	AccordingToPlan
}
