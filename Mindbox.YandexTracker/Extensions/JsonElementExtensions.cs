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

	public static Collection<FieldInfo> ToFieldInfoCollection(this JsonElement element)
	{
		var collection = new Collection<FieldInfo>();

		foreach (var item in element.EnumerateArray())
		{
			collection.Add(new FieldInfo
			{
				Id = item.GetProperty("id").GetString() ?? string.Empty,
				Key = item.GetProperty("key").GetString() ?? string.Empty,
				Display = item.GetProperty("display").GetString() ?? string.Empty
			});
		}

		return collection;
	}
}