namespace Mindbox.YandexTracker;

public class Component
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public string? Queue { get; set; }
	public string? Description { get; set; }
	public required bool AssignAuto { get; set; }
	public UserInfo? Lead { get; set; }
}
