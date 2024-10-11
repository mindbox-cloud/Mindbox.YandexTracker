namespace Mindbox.YandexTracker;

internal static class StringExtensions
{
	public static string? TrimAndMakeNullIfEmpty(this string value)
	{
		return !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;
	}

	public static string ToYandexRouteSegment(this ProjectEntityType value)
	{
#pragma warning disable CA1308 // Требует заменить ToLowerInvariant на ToUpperInvariant, но Яндексу нужен lower
		return value.ToString().ToLowerInvariant();
#pragma warning restore CA1308
	}
}
