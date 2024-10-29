using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

internal sealed record FieldInfo
{
	[JsonPropertyName("id")]
	public required string Id { get; init; }

	[JsonPropertyName("key")]
	public string? Key { get; init; }

	[JsonPropertyName("display")]
	public required string Display { get; init; }
}
