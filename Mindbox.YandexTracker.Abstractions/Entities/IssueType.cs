using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Тип задачи
/// </summary>
public sealed record IssueType
{
	/// <summary>
	/// Идентификатор типа
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; init; }

	/// <summary>
	/// Ключ
	/// </summary>
	[JsonPropertyName("key")]
	public required string Key { get; init; }

	/// <summary>
	/// Название
	/// </summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>
	/// Описание
	/// </summary>
	[JsonPropertyName("description")]
	public string Description { get; init; } = null!;
}
