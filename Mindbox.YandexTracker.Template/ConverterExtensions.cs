using System.Globalization;

namespace Mindbox.YandexTracker.Template;

public static class ConverterExtensions
{
	public static UserShortInfo ToUserShortInfo(this UserDetailedInfo user)
	{
		return new UserShortInfo
		{
			Display = user.Display,
			Id = user.Uid.ToString(CultureInfo.InvariantCulture)
		};
	}
}