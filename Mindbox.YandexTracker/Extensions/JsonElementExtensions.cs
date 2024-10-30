using System.Collections.ObjectModel;
using System.Text.Json;

namespace Mindbox.YandexTracker;

internal static class JsonElementExtensions
{
	public static UserShortInfo ToUserShortInfo(this JsonElement element)
	{
		return new UserShortInfo
		{
			Display = element.GetProperty("display").GetString() ?? string.Empty,
			Id = element.GetProperty("id").GetInt64()
		};
	}

	public static Collection<UserShortInfo> ToUserCollection(this JsonElement element)
	{
		var collection = new Collection<UserShortInfo>();

		foreach (var item in element.EnumerateArray())
		{
			collection.Add(ToUserShortInfo(item));
		}

		return collection;
	}

	public static Collection<string> ToStringCollection(this JsonElement element)
	{
		var collection = new Collection<string>();

		foreach (var item in element.EnumerateArray())
		{
			collection.Add(item.GetString()!);
		}

		return collection;
	}

	public static ProjectEntityStatus ToProjectEntityStatus(this JsonElement element)
	{
		return element.GetString()?.ToProjectStatus() ?? default;
	}
}