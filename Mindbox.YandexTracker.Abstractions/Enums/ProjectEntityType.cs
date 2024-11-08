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
/// Тип проекта/портфеля
/// </summary>
public enum ProjectEntityType
{
	/// <summary>
	/// Проект
	/// </summary>
	Project,

	/// <summary>
	/// Портфель — это набор проектов и других портфелей, объединенных в одно направление
	/// </summary>
	Portfolio
}
