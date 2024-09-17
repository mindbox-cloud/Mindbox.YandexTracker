using System;

namespace Mindbox.YandexTracker;
public class NoRetryException : GitlabException
{
	public NoRetryException(string message) : base(message)
	{
	}

	public NoRetryException(string message, Exception innerException) : base(message, innerException)
	{
	}
}