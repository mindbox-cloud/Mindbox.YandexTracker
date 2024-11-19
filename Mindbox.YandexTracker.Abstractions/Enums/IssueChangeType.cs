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
/// Представляет тип изменения, внесенного в задачу.
/// </summary>
public enum IssueChangeType
{
	/// <summary>
	/// Задача изменена.
	/// </summary>
	IssueUpdated,

	/// <summary>
	/// Задача создана.
	/// </summary>
	IssueCreated,

	/// <summary>
	/// Задача перемещена в другую очередь.
	/// </summary>
	IssueMoved,

	/// <summary>
	/// Создан клон задачи.
	/// </summary>
	IssueCloned,

	/// <summary>
	/// Добавлен комментарий к задаче.
	/// </summary>
	IssueCommentAdded,

	/// <summary>
	/// Изменен комментарий к задаче.
	/// </summary>
	IssueCommentUpdated,

	/// <summary>
	/// Удален комментарий к задаче.
	/// </summary>
	IssueCommentRemoved,

	/// <summary>
	/// Добавлена запись о затраченном времени.
	/// </summary>
	IssueWorklogAdded,

	/// <summary>
	/// Изменена запись о затраченном времени.
	/// </summary>
	IssueWorklogUpdated,

	/// <summary>
	/// Удалена запись о затраченном времени.
	/// </summary>
	IssueWorklogRemoved,

	/// <summary>
	/// За комментарий проголосовали.
	/// </summary>
	IssueCommentReactionAdded,

	/// <summary>
	/// Отозван голос за комментарий.
	/// </summary>
	IssueCommentReactionRemoved,

	/// <summary>
	/// За задачу проголосовали.
	/// </summary>
	IssueVoteAdded,

	/// <summary>
	/// Отозван голос за задачу.
	/// </summary>
	IssueVoteRemoved,

	/// <summary>
	/// Создана связь с другой задачей.
	/// </summary>
	IssueLinked,

	/// <summary>
	/// Изменен тип связи с другой задачей.
	/// </summary>
	IssueLinkChanged,

	/// <summary>
	/// Удалена связь с другой задачей.
	/// </summary>
	IssueUnlinked,

	/// <summary>
	/// Изменена резолюция связанной задачи.
	/// </summary>
	RelatedIssueResolutionChanged,

	/// <summary>
	/// К задаче прикреплен файл.
	/// </summary>
	IssueAttachmentAdded,

	/// <summary>
	/// Прикрепленный к задаче файл удален.
	/// </summary>
	IssueAttachmentRemoved,

	/// <summary>
	/// Изменен статус задачи.
	/// </summary>
	IssueWorkflow
}