namespace Mindbox.YandexTracker;

public sealed record CreateQueueLocalFieldResponse
{
	public required string Id { get; init; }

	public required string Type { get; init; }

	public required string Name { get; init; }

	public string? Description { get; init; }

	public required string Key { get; init; }

	public required SchemaInfoDto Schema { get; init; }

	public bool Readonly { get; init; }

	public bool Options { get; init; }

	public bool Suggest { get; init; }

	public int Order { get; init; }

	public required FieldInfo Category { get; init; }

	public required ProviderInfoDto QueryProvider { get; init; }
}