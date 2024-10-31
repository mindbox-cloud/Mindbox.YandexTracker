using System.Collections.ObjectModel;
using System.Text.Json;

namespace Mindbox.YandexTracker;

public sealed record GetIssueFieldsResponse
{
	public required string Id { get; init; }

	public required string Key { get; init; }

	public required string Name { get; init; }

	public string? Description { get; init; }

	public bool Readonly { get; init; }

	public bool Options { get; init; }

	public bool Suggest { get; init; }

	public ProviderInfoDto? SuggestProvider { get; init; }

	public OptionsProviderInfoDto? OptionsProvider { get; init; }

	public ProviderInfoDto? QueryProvider { get; init; }

	public int Order { get; init; }

	public required FieldInfo Category { get; init; }

	public required string Type { get; init; }

	public required SchemaInfoDto Schema { get; init; }
}

public class ProviderInfoDto
{
	public required string Type { get; init; }
}

public class SchemaInfoDto
{
	public required string Type { get; init; }

	public string? Items { get; init; }

	public bool Required { get; init; }
}

public class OptionsProviderInfoDto
{
	public required string Type { get; init; }

	public Collection<JsonElement> Values { get; init; } = [];
}