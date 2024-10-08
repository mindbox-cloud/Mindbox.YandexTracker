namespace Mindbox.YandexTracker;

internal static class StringExtensions
{
	public static string? TrimAndMakeNullIfEmpty(this string value)
	{
		return !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;
	}
}
