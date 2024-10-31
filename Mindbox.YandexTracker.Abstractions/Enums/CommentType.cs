namespace Mindbox.YandexTracker;

/// <summary>
/// Тип комментария
/// </summary>
public enum CommentType
{
	/// <summary>
	/// Отправлен через интерфейс Tracker
	/// </summary>
	Standard,

	/// <summary>
	/// Создан из входящего письма
	/// </summary>
	Incoming,

	/// <summary>
	/// Создан из исходящего письма
	/// </summary>
	Outcoming
}
