using System;
using System.Collections.Generic;

namespace Mindbox.YandexTracker.Template;

internal static class CollectionExtensions
{
	public static int GetCollectionHashCode<T>(this IReadOnlyList<T> collection)
	{
		var hashCode = 0;

		foreach (var item in collection)
		{
			hashCode = HashCode.Combine(hashCode, item);
		}

		return hashCode;
	} 
}
