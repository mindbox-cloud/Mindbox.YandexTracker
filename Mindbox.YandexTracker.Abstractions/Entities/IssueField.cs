using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public class IssueField
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
	public bool Readonly { get; set; }
	public bool Options { get; set; }
	public bool Suggest { get; set; }
	public OptionsProviderInfo? OptionsProvider { get; set; }
	public required string QueryProvider { get; set; }
	public string? SuggestProvider { get; set; }
	public int Order { get; set; }
	public required FieldInfo Category { get; set; }
}

public class OptionsProviderInfo
{
	public required string Type { get; set; }
	public Collection<string> Values { get; set; } = [];
}
