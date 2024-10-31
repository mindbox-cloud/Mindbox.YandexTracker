using System;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Mindbox.YandexTracker;

internal static class JsonElementExtensions
{
	public static UserShortInfo ToUserShortInfo(this JsonElement element)
	{
		var display = element.GetProperty("display").GetString();
		var id = element.GetProperty("id").GetString();

		return new UserShortInfo
		{
			Display = display ?? throw new ArgumentNullException(display),
			Id = id ?? throw new ArgumentNullException(id)
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
			var display = item.GetProperty("display").GetString();
			var id = item.GetProperty("id").GetString();

			collection.Add(new FieldInfo
			{
				Id = display ?? throw new ArgumentNullException(display),
				Key = item.GetProperty("key").GetString(),
				Display = id ?? throw new ArgumentNullException(id)
			});
		}

		return collection;
	}
}