using System;
using System.Collections.Generic;

namespace Mindbox.YandexTracker.Template;

internal static class CollectionExtensions
{
	public static int GetCollectionHashCode<T>(this IReadOnlyList<T> collection)
	{
		var hashCode = 0;

		for (var i = 0; i < collection.Count; i++)
		{
			var item = collection[i];
			hashCode = HashCode.Combine(hashCode, item);
		}

		return hashCode;
	}
}
