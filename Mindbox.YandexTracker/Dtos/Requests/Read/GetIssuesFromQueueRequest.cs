using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

internal sealed record GetIssuesFromQueueRequest
{
	[JsonPropertyName("queue")]
	public required string QueueKey { get; init; }
}
