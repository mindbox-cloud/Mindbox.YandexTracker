using System.Threading.Tasks;
using System;

namespace Mindbox.YandexTracker;
internal static class RetryHelpers
{
	public static async Task<TResult> RetryOnExceptionAsync<TResult>(Func<Task<TResult>> func, int retryCount)
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
				if (e is NoRetryException)
				{
					throw;
				}
				if (num > retryCount)
				{
					throw new GitlabException($"{retryCount} error responses in a row.", e);
				}
			}

			++num;
		}
	}
}
