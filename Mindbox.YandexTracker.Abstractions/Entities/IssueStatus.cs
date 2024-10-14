using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Статус задачи
/// </summary>
[DataContract]
public sealed record IssueStatus
{
	/// <summary>
	/// Идентификатор статуса
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; init; }

	/// <summary>
	/// Ключ статуса
	/// </summary>
	[DataMember(Name = "key")]
	public required string Key { get; init; }

	/// <summary>
	/// Название статуса
	/// </summary>
	[DataMember(Name = "name")]
	public required string Name { get; init; }

	/// <summary>
	/// Описание статуса
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; init; } = null!;

	/// <summary>
	/// Тип статуса задачи
	/// </summary>
	[DataMember(Name = "type")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public IssueStatusType Type { get; init; }
}
