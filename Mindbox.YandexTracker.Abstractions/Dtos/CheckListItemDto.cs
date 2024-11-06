namespace Mindbox.YandexTracker;

public sealed record CheckListItemDto
{
	public required string Id { get; init; }
	public string? Text { get; init; }
	public bool Checked { get; init; }
	public ChecklistItemType ChecklistItemType { get; set; }
}