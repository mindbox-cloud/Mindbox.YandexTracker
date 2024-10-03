using System;

namespace Mindbox.YandexTracker;

internal static class OtherExtensions
{
	public static string? TrimAndMakeNullIfEmpty(this string value)
	{
		return !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;
	}

	public static TOut Transform<TIn, TOut>(this TIn @this, Func<TIn, TOut> transformer)
	{
		ArgumentNullException.ThrowIfNull(transformer);

		return transformer(@this);
	}
}
