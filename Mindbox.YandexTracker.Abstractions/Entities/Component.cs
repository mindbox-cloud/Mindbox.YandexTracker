namespace Mindbox.YandexTracker;

/// <summary>
/// Компонент
/// </summary>
public sealed record Component
{
	/// <summary>
	/// Идентификатор компонента
	/// </summary>
	public int Id { get; init; }

	/// <summary>
	/// Название компонента
	/// </summary>
	public required string Name { get; init; }

	/// <summary>
	/// Объект с информацией об очереди компонента
	/// </summary>
	public string? Queue { get; init; }

	/// <summary>
	/// Текстовое описание компонента
	/// </summary>
	public string? Description { get; init; }

	/// <summary>
	/// Автоматически назначить владельца компонента исполнителем для новых задач с этим компонентом:<br/>
	/// true — назначить;<br/>
	/// false — не назначать.
	/// </summary>
	public required bool AssignAuto { get; init; }

	/// <summary>
	/// Блок с информацией о владельце компонента
	/// </summary>
	public UserShortInfo? Lead { get; init; }
}
