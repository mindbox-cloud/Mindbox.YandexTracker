using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Приоритет
/// </summary>
public enum Priority
{
	/// <summary>
	/// Незначительный
	/// </summary>
	[JsonPropertyName("trivial")]
	Trivial,

	/// <summary>
	/// Низкий
	/// </summary>
	[JsonPropertyName("minor")]
	Minor,

	/// <summary>
	/// Средний
	/// </summary>
	[JsonPropertyName("normal")]
	Normal,

	/// <summary>
	/// Критичный
	/// </summary>
	[JsonPropertyName("critical")]
	Critical,

	/// <summary>
	/// Блокер
	/// </summary>
	[JsonPropertyName("blocker")]
	Blocker
}
