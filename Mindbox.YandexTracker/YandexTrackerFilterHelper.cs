namespace Mindbox.YandexTracker;

/// <summary>
/// https://yandex.cloud/ru/docs/tracker/user/query-filter#query-functions
/// </summary>
public static class YandexTrackerFilterFunctions
{
	public const string Empty = "empty()";
	public const string NotEmpty = "notEmpty()";
	public const string Now = "now()";
	public const string Me = "me()";
	public const string Today = "today()";
	public const string Week = "week()";
	public const string Month = "month()";
	public const string Quarter = "quarter()";
	public const string Year = "year()";
	public const string Unresolved = "unresolved()";

	public static string Group(string groupName) => $"group(value: \"{groupName}\")";
}
