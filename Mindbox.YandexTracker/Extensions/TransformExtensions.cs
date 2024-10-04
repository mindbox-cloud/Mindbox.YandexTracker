using System;

namespace Mindbox.YandexTracker;

internal static class TransformExtensions
{
	public static TOut Transform<TIn, TOut>(this TIn @this, Func<TIn, TOut> transformer)
	{
		ArgumentNullException.ThrowIfNull(transformer);

		return transformer(@this);
	}
}
