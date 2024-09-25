using System;

namespace Mindbox.YandexTracker;

/// <summary>
/// Дополнительные поля, которые будут включены в ответ. Возможен множественный выбор.
/// </summary>
[Flags]
public enum CommentExpandData
{
	None = 0,
	Attachments = 0x0001,
	Html = 0x0002,
	All = Attachments | Html,
}