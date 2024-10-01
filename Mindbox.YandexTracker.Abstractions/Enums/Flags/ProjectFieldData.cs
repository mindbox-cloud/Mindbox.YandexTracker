using System;

namespace Mindbox.YandexTracker;

/// <summary>
/// Дополнительные поля, которые будут включены в ответ. Возможен множественный выбор.
/// </summary>
[Flags]
public enum ProjectFieldData
{
	None = 0,
	Summary = 0x0001,
	Description = 0x0002,
	Author = 0x0004,
	Lead = 0x0008,
	TeamUsers = 0x0010,
	Clients = 0x0020,
	Followers = 0x0040,
	Start = 0x0080,
	End = 0x0100,
	ChecklistItems = 0x0200,
	Tags = 0x0400,
	ParentEntity = 0x0800,
	TeamAccess = 0x1000,
	Quarter = 0x2000,
	EntityStatus = 0x4000,
	IssueQueues = 0x8000
}
