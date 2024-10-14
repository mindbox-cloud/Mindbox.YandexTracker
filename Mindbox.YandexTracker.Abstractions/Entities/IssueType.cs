using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Тип задачи
/// </summary>
[DataContract]
public sealed record IssueType
{
	/// <summary>
	/// Идентификатор типа
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; init; }

	/// <summary>
	/// Ключ
	/// </summary>
	[DataMember(Name = "key")]
	public required string Key { get; init; }

	/// <summary>
	/// Название
	/// </summary>
	[DataMember(Name = "name")]
	public required string Name { get; init; }

	/// <summary>
	/// Описание
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; init; } = null!;
}
