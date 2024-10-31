using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Резолюция — это атрибут задачи, который обозначает причину ее закрытия
/// </summary>
[DataContract]
public sealed record GetResolutionResponse
{
	/// <summary>
	/// Идентификатор
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
