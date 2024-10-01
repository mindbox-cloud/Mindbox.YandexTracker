using System;

namespace Mindbox.YandexTracker;

/// <summary>
/// Дополнительные поля, которые будут включены в ответ. Возможен множественный выбор.
/// </summary>
[Flags]
public enum QueuesExpandData
{
	None = 0,
	Projects = 0x0001,
	Components = 0x0002,
	Versions = 0x0004,
	Types = 0x0008,
	Team = 0x0010,
	Workflows = 0x0020
}