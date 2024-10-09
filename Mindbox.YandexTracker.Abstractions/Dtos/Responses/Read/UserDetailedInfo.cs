using System;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record UserDetailedInfo
{
	[DataMember(Name = "uid")]
	public required string Id { get; init; }

	[DataMember(Name = "login")]
	public required string Login { get; init; }

	[DataMember(Name = "trackerUid")]
	public required string TrackerUid { get; init; }

	[DataMember(Name = "passportUid")]
	public required string PassportUid { get; init; }

	[DataMember(Name = "cloudUid")]
	public required string CloudUid { get; init; }

	[DataMember(Name = "firstName")]
	public required string FirstName { get; init; }

	[DataMember(Name = "lastName")]
	public required string LastName { get; init; }

	[DataMember(Name = "display")]
	public required string Display { get; init; }

	[DataMember(Name = "email")]
	public required string Email { get; init; }

	[DataMember(Name = "external")]
	public bool External { get; init; }

	[DataMember(Name = "hasLicense")]
	public bool HasLicense { get; init; }

	[DataMember(Name = "dismissed")]
	public bool Dismissed { get; init; }

	[DataMember(Name = "useNewFilters")]
	public bool UseNewFilters { get; init; }

	[DataMember(Name = "disableNotifications")]
	public bool DisableNotifications { get; init; }

	[DataMember(Name = "firstLoginDate")]
	public DateTime FirstLoginDate { get; init; }

	[DataMember(Name = "lastLoginDate")]
	public DateTime LastLoginDate { get; init; }

	[DataMember(Name = "welcomeMailSent")]
	public bool WelcomeMailSent { get; init; }
}
