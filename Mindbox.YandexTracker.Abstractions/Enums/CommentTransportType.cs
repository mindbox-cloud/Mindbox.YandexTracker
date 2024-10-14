using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Способ добавления комментария
/// </summary>
[DataContract]
public enum CommentTransportType
{
	/// <summary>
	/// Через интерфейс Tracker
	/// </summary>
	[EnumMember(Value = "internal")]
	Internal,

	/// <summary>
	/// Через письмо
	/// </summary>
	[EnumMember(Value = "email")]
	Email
}
