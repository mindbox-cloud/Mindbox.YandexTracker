using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

public sealed record GetProjectsRequest
{
	public string? Input { get; init; }

	public ProjectFieldsDto? Filter { get; init; }

	public string? OrderBy { get; init; }

	public bool? OrderAsc { get; init; }

	public bool? RootOnly { get; init; }
}
