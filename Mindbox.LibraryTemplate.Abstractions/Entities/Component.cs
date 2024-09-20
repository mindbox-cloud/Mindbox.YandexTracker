namespace Mindbox.YandexTracker;
public class Component
{
	public required string Name { get; set; }
	public string? Queue { get; set; }
	public string? Description { get; set; }
	public required bool AssignAuto { get; set; }
	public required Account Lead { get; set; }
}
