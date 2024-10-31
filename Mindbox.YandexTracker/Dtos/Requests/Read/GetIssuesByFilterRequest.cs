using System.Collections.Generic;

namespace Mindbox.YandexTracker;

internal sealed record GetIssuesByFilterRequest
{
	public required Dictionary<string, object> Filter { get; init; }
}
