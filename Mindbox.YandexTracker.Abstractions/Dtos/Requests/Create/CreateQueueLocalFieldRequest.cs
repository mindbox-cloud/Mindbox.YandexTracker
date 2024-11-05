using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

public sealed record CreateQueueLocalFieldRequest
{
	public required string Id { get; init; }

	public required QueueLocalFieldName Name { get; init; }

	public required string Category { get; init; }

	public QueueLocalFieldType Type { get; init; }

	public OptionsProviderInfoDto? OptionsProvider { get; init; }

	public int? Order { get; init; }

	public string? Description { get; init; }

	public bool? Readonly { get; init; }

	public bool? Visible { get; init; }

	public bool? Hidden { get; init; }

	public bool? Container { get; init; }
}