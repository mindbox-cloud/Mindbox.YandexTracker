using System;
using System.Threading.Tasks;

namespace Mindbox.YandexTracker.Tests;

public static class SafeExecutor
{
	public static async Task ExecuteAsync(Func<Task> action)
	{
		try
		{
			await action();
		}
		catch
		{
			// ignored
		}
	}
}