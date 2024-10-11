using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Тип комментария
/// </summary>
[DataContract]
public enum CommentType
{
	/// <summary>
	/// Отправлен через интерфейс Tracker
	/// </summary>
	[EnumMember(Value = "standard")]
	Standard,

	/// <summary>
	/// Создан из входящего письма
	/// </summary>
	[EnumMember(Value = "incoming")]
	Incoming,

	/// <summary>
	/// Создан из исходящего письма
	/// </summary>
	[EnumMember(Value = "outcoming")]
	Outcoming
}
