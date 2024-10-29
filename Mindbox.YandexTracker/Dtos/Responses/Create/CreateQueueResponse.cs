using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

internal sealed record CreateQueueResponse
{
	[JsonPropertyName("id")]
	public required string Id { get; init; }

	[JsonPropertyName("key")]
	public required string Key { get; init; }

	[JsonPropertyName("name")]
	public required string Name { get; init; }

	[JsonPropertyName("lead")]
	public required FieldInfo Lead { get; init; }

	[JsonPropertyName("assignAuto")]
	public bool AssignAuto { get; init; }

	[JsonPropertyName("allowExternals")]
	public bool AllowExternals { get; init; }

	[JsonPropertyName("defaultType")]
	public required FieldInfo DefaultType { get; init; }

	[JsonPropertyName("defaultPriority")]
	public required FieldInfo DefaultPriority { get; init; }
}
