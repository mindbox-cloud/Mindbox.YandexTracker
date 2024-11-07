using System.Collections.Generic;

namespace Mindbox.YandexTracker;

public sealed record GetIssuesFromKeysRequest
{
	public required IReadOnlyCollection<string>? Keys { get; init; }
}
