using System.Collections.Generic;

namespace Mindbox.YandexTracker;

public sealed record GetIssuesFromKeysRequest
{
	public IReadOnlyCollection<string> Keys { get; init; } = [];
}
