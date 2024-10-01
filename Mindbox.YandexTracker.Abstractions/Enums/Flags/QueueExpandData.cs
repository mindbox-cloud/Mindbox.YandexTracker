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