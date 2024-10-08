using System;

namespace Mindbox.YandexTracker;

/// <summary>
/// Дополнительные поля, которые будут включены в ответ. Возможен множественный выбор.
/// </summary>
[Flags]
public enum IssueExpandData
{
	None = 0,
	Transitions = 0x0001,
	Attachments = 0x0002
}