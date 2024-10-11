using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Приоритет
/// </summary>
[DataContract]
public enum Priority
{
	/// <summary>
	/// Незначительный
	/// </summary>
	[EnumMember(Value = "trivial")]
	Trivial,

	/// <summary>
	/// Низкий
	/// </summary>
	[EnumMember(Value = "minor")]
	Minor,

	/// <summary>
	/// Средний
	/// </summary>
	[EnumMember(Value = "normal")]
	Normal,

	/// <summary>
	/// Критичный
	/// </summary>
	[EnumMember(Value = "critical")]
	Critical,

	/// <summary>
	/// Блокер
	/// </summary>
	[EnumMember(Value = "blocker")]
	Blocker
}
