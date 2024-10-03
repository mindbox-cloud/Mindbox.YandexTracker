using System;

namespace Mindbox.YandexTracker;

public class YandexTrackerRetryLimitExceededException : YandexTrackerException
{
	public YandexTrackerRetryLimitExceededException(string message) : base(message)
	{
	}

	public YandexTrackerRetryLimitExceededException(string message, Exception innerException) : base(message, innerException)
	{
	}
}