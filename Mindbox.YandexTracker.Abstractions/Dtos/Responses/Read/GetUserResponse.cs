using System;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record GetUserResponse
{
	[DataMember(Name = "uid")]
	public int Id { get; set; }

	[DataMember(Name = "login")]
	public required string Login { get; set; }

	[DataMember(Name = "trackerUid")]
	public int TrackerUid { get; set; }

	[DataMember(Name = "passportUid")]
	public int PassportUid { get; set; }

	[DataMember(Name = "cloudUid")]
	public required string CloudUid { get; set; }

	[DataMember(Name = "firstName")]
	public required string FirstName { get; set; }

	[DataMember(Name = "lastName")]
	public required string LastName { get; set; }

	[DataMember(Name = "display")]
	public required string Display { get; set; }

	[DataMember(Name = "email")]
	public required string Email { get; set; }

	[DataMember(Name = "external")]
	public bool External { get; set; }

	[DataMember(Name = "hasLicense")]
	public bool HasLicense { get; set; }

	[DataMember(Name = "dismissed")]
	public bool Dismissed { get; set; }

	[DataMember(Name = "useNewFilters")]
	public bool UseNewFilters { get; set; }

	[DataMember(Name = "disableNotifications")]
	public bool DisableNotifications { get; set; }

	[DataMember(Name = "firstLoginDate")]
	public DateTime FirstLoginDate { get; set; }

	[DataMember(Name = "lastLoginDate")]
	public DateTime LastLoginDate { get; set; }

	[DataMember(Name = "welcomeMailSent")]
	public bool WelcomeMailSent { get; set; }
}
