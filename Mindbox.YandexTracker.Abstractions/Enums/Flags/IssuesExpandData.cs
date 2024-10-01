using System;

namespace Mindbox.YandexTracker;

/// <summary>
/// Дополнительные поля, которые будут включены в ответ. Возможен множественный выбор.
/// </summary>
[Flags]
public enum IssuesExpandData
{
	None = 0,
	Transitions = 0x1,
	Attachments = 0x2,
	Links = 0x4
}
