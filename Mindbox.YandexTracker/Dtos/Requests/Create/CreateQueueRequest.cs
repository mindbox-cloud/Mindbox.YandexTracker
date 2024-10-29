using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

internal sealed record CreateQueueRequest
{
	[JsonPropertyName("key")]
	public required string Key { get; init; }

	[JsonPropertyName("name")]
	public required string Name { get; init; }

	[JsonPropertyName("lead")]
	public required string LeadId { get; init; }

	[JsonPropertyName("defaultType")]
	public required string DefaultType { get; init; }

	[JsonPropertyName("defaultPriority")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public Priority DefaultPriority { get; init; }

	[JsonPropertyName("issueTypesConfig")]
	public Collection<CreateIssueTypeConfigDto> IssutTypesConfig { get; init; } = [];
}

internal sealed record CreateIssueTypeConfigDto
{
	[JsonPropertyName("issueType")]
	public required string IssueType { get; init; }

	[JsonPropertyName("workflow")]
	public required string Workflow { get; init; }

	[JsonPropertyName("resolutions")]
	public Collection<string> Resolutions { get; init; } = [];
}