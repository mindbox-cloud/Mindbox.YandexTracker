using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record IssueField
{
	public required string Id { get; init; }
	public required string Key { get; init; }
	public required string Name { get; init; }
	public string? Description { get; init; }
	public bool Readonly { get; init; }
	public bool Options { get; init; }
	public bool Suggest { get; init; }
	public required OptionsProviderInfo OptionsProvider { get; init; }
	public string? QueryProvider { get; init; }
	public string? SuggestProvider { get; init; }
	public int Order { get; init; }
	public required string CategoryId { get; init; }
	public required string Schema { get; init; }
}

public sealed record OptionsProviderInfo
{
	public required string Type { get; init; }
	public Collection<string> Values { get; init; } = [];
}
