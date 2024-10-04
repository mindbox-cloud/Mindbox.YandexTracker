using System.Threading.Tasks;
using System;
using System.Threading;

namespace Mindbox.YandexTracker;

public static class RetryHelpers
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

	public static async Task RetryOnExceptionAsync(
		Func<Task> func,
		int retryCount,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(func);

		var num = 1;

		while (true)
		{
			try
			{
				await func();
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
