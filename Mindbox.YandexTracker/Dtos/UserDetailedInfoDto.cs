using System;
using System.Text.Json.Serialization;
using Mindbox.YandexTracker.Helpers;

namespace Mindbox.YandexTracker;

internal sealed record UserDetailedInfoDto
{
	[JsonPropertyName( "uid")]
	[JsonConverter(typeof(NumberToStringConverter))]
	public required string Id { get; init; }

	[JsonPropertyName("login")]
	public required string Login { get; init; }

	[JsonPropertyName("trackerUid")]
	[JsonConverter(typeof(NumberToStringConverter))]
	public required string TrackerUid { get; init; }

	[JsonPropertyName("passportUid")]
	[JsonConverter(typeof(NumberToStringConverter))]
	public required string PassportUid { get; init; }

	[JsonPropertyName("cloudUid")]
	public required string CloudUid { get; init; }

	[JsonPropertyName("firstName")]
	public required string FirstName { get; init; }

	[JsonPropertyName("lastName")]
	public required string LastName { get; init; }

	[JsonPropertyName("display")]
	public required string Display { get; init; }

	[JsonPropertyName("email")]
	public required string Email { get; init; }

	[JsonPropertyName("external")]
	public bool External { get; init; }

	[JsonPropertyName("hasLicense")]
	public bool HasLicense { get; init; }

	[JsonPropertyName("dismissed")]
	public bool Dismissed { get; init; }

	[JsonPropertyName("useNewFilters")]
	public bool UseNewFilters { get; init; }

	[JsonPropertyName("disableNotifications")]
	public bool DisableNotifications { get; init; }

	[JsonPropertyName("firstLoginDate")]
	[JsonConverter(typeof(DateTimeConverter))]
	public DateTime FirstLoginDateUtc { get; init; }

	[JsonPropertyName("lastLoginDate")]
	[JsonConverter(typeof(DateTimeConverter))]
	public DateTime LastLoginDateUtc { get; init; }

	[JsonPropertyName("welcomeMailSent")]
	public bool WelcomeMailSent { get; init; }
}