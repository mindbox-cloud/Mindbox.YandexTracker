using System;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

internal sealed record UserDetailedInfoDto
{
	[JsonPropertyName("uid")]
	public required long Id { get; init; }

	public required string Login { get; init; }

	public required long TrackerUid { get; init; }

	public long? PassportUid { get; init; }

	public string? CloudUid { get; init; }

	public required string FirstName { get; init; }

	public required string LastName { get; init; }

	public required string Display { get; init; }

	public required string Email { get; init; }

	public bool External { get; init; }

	public bool HasLicense { get; init; }

	public bool Dismissed { get; init; }

	public bool UseNewFilters { get; init; }

	public bool DisableNotifications { get; init; }

	[JsonPropertyName("firstLoginDate")]
	public DateTime FirstLoginDateUtc { get; init; }

	[JsonPropertyName("lastLoginDate")]
	public DateTime LastLoginDateUtc { get; init; }

	public bool WelcomeMailSent { get; init; }
}