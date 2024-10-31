using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

internal sealed record GetProjectsRequest
{
	public string? Input { get; init; }

	public ProjectFieldsDto? Filter { get; init; }

	public string? OrderBy { get; init; }

	[JsonPropertyName("orderAsc")]
	public bool? OrderAscending { get; init; }

	public bool? RootOnly { get; init; }
}
