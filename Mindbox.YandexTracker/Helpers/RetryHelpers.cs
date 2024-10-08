using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mindbox.YandexTracker.Helpers;

internal static class RetryHelpers
{
	public static async Task<TResult> RetryOnExceptionAsync<TResult>(
		Func<Task<TResult>> func,
		int retryCount,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(func);

		var num = 1;

		while (true)
		{
			cancellationToken.ThrowIfCancellationRequested();

			try
			{
				return await func();
			}
			catch (Exception e)
			{
				if (e is YandexTrackerRetryLimitExceededException)
				{
					throw;
				}
				if (num > retryCount)
				{
					throw new YandexTrackerException($"{retryCount} error responses in a row.", e);
				}
			}

			++num;
		}
	}
}
