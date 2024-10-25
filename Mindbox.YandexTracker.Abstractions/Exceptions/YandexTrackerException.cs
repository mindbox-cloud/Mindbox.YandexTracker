using System;
using System.Net;

namespace Mindbox.YandexTracker;

public class YandexTrackerException(
	string message,
	HttpStatusCode statusCode,
	Exception? innerException = null)
	: Exception(message, innerException)
{
	public HttpStatusCode StatusCode { get; init; } = statusCode;
}
