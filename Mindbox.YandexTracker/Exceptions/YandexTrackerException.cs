using System;

namespace Mindbox.YandexTracker;

public class YandexTrackerException : Exception
{
	public YandexTrackerException(string message) : base(message)
	{
	}

	public YandexTrackerException()
	{
	}

	public YandexTrackerException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
