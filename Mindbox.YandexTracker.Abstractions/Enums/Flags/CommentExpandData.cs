using System;

namespace Mindbox.YandexTracker;

/// <summary>
/// Дополнительные поля, которые будут включены в ответ. Возможен множественный выбор.
/// </summary>
[Flags]
public enum CommentExpandData
{
#pragma warning disable CA1008
	Unspecified = 0x0000,
#pragma warning restore CA1008
	Attachments = 0x0001,
	Html = 0x0002,
	All = Attachments | Html,
}