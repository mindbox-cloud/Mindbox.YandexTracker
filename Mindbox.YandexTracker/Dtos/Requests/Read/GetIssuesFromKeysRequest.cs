using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

internal sealed record GetIssuesFromKeysRequest
{
	public Collection<string> Keys { get; init; } = [];
}
