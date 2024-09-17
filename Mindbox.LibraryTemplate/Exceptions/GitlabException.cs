using System;

namespace Mindbox.YandexTracker;
public class GitlabException : Exception
{
	public GitlabException(string message) : base(message)
	{
	}

	public GitlabException()
	{
	}

	public GitlabException(string message, Exception innerException) : base(message, innerException)
	{
	}
}