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
using System.Linq;

namespace Mindbox.YandexTracker;

public sealed record IssuesFilterDto
{
	/// <summary>
	/// Дата и время последнего добавленного комментария
	/// </summary>
	public DateTime? LastCommentUpdatedAt { get; init; }

	/// <summary>
	/// Название задачи
	/// </summary>
	public IReadOnlyCollection<string>? Summary { get; init; }

	/// <summary>
	/// Идентификатор родительской задачи
	/// </summary>
	public IReadOnlyCollection<string>? Parent { get; init; }

	/// <summary>
	/// Идентификатор последнего сотрудника, изменившего задачу
	/// </summary>
	public IReadOnlyCollection<string>? UpdatedBy { get; init; }

	/// <summary>
	/// Описание задачи
	/// </summary>
	public IReadOnlyCollection<string>? Description { get; init; }

	/// <summary>
	/// Массив с информацией о спринте
	/// </summary>
	public IReadOnlyCollection<string>? Sprint { get; init; }

	/// <summary>
	/// Тип задачи
	/// </summary>
	public IReadOnlyCollection<string>? Type { get; init; }

	/// <summary>
	/// Приоритет задачи
	/// </summary>
	public IReadOnlyCollection<Priority>? Priority { get; init; }

	/// <summary>
	/// Дата и время создания задачи
	/// </summary>
	public DateTime? CreatedAt { get; init; }

	/// <summary>
	/// Массив идентификаторов наблюдателей задачи
	/// </summary>
	public IReadOnlyCollection<string>? Followers { get; init; }

	/// <summary>
	/// Идентификатор создателя задачи
	/// </summary>
	public IReadOnlyCollection<string>? CreatedBy { get; init; }

	/// <summary>
	/// Количество голосов за задачу
	/// </summary>
	public int? Votes { get; init; }

	/// <summary>
	/// Идентификатор исполнителя задачи
	/// </summary>
	public IReadOnlyCollection<string>? Assignee { get; init; }

	/// <summary>
	/// Идентификатор проекта задачи
	/// </summary>
	public IReadOnlyCollection<string>? Project { get; init; }

	/// <summary>
	/// Идентификатор очереди задачи
	/// </summary>
	public IReadOnlyCollection<string>? Queue { get; init; }

	/// <summary>
	/// Дата и время последнего обновления задачи
	/// </summary>
	public DateTime? UpdatedAt { get; init; }

	/// <summary>
	/// Статус задачи
	/// </summary>
	public IReadOnlyCollection<string>? Status { get; init; }

	/// <summary>
	/// Предыдущий статус задачи
	/// </summary>
	public IReadOnlyCollection<string>? PreviousStatus { get; init; }

	public override int GetHashCode()
	{
		var hashCodePart1 = HashCode.Combine(
			LastCommentUpdatedAt,
			CreatedAt,
			UpdatedAt);

		var collectionHashCode = 0;
		CombineCollectionHashCode(Summary);
		CombineCollectionHashCode(Parent);
		CombineCollectionHashCode(UpdatedBy);
		CombineCollectionHashCode(Description);
		CombineCollectionHashCode(Sprint);
		CombineCollectionHashCode(Type);
		CombineCollectionHashCode(Priority);
		CombineCollectionHashCode(Followers);
		CombineCollectionHashCode(CreatedBy);
		CombineCollectionHashCode(Assignee);
		CombineCollectionHashCode(Project);
		CombineCollectionHashCode(Queue);
		CombineCollectionHashCode(Status);
		CombineCollectionHashCode(PreviousStatus);

		return HashCode.Combine(hashCodePart1, collectionHashCode);

		void CombineCollectionHashCode<T>(IEnumerable<T>? collection)
		{
			if (collection is null)
				return;
			collectionHashCode = collection.Aggregate(collectionHashCode, (current, item) => HashCode.Combine(item, current));
		}
	}
}
