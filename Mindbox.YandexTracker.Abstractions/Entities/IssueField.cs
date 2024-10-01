using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record IssueField
{
	public required string Id { get; set; }
	public required string Key { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
	public bool Readonly { get; set; }
	public bool Options { get; set; }
	public bool Suggest { get; set; }
	public required OptionsProviderInfo OptionsProvider { get; set; }
	public required string QueryProvider { get; set; }
	public string? SuggestProvider { get; set; }
	public int Order { get; set; }
	public required string CategoryId { get; set; }
	public required string Schema { get; set; }
}

public sealed record OptionsProviderInfo
{
	public required string Type { get; set; }
	public Collection<string> Values { get; set; } = [];
}
