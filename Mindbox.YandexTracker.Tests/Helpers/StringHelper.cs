using System.Linq;
using System.Security.Cryptography;

namespace Mindbox.YandexTracker.Tests;

internal static class StringHelper
{
	private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	public static string GetUniqueString(int length)
	{
		return new string(
			Enumerable.Repeat(Chars, length)
				.Select(s => s[RandomNumberGenerator.GetInt32(s.Length)])
					.ToArray());
	}
}
