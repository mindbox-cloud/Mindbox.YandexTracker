using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record GetIssuesFromKeysRequest
{
	public Collection<string> Keys { get; init; } = [];
}
