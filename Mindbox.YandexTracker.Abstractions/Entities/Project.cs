using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public sealed record Project
{
	public string Id { get; init; } = null!;
	public int ShortId { get; init; }
	public ProjectEntityType ProjectType { get; init; }
	public UserShortInfo CreatedBy { get; init; } = null!;
	public DateTime CreatedAt { get; init; }
	public DateTime UpdatedAt { get; init; }
	public string? Summary { get; init; }
	public string? Description { get; init; }
	public UserShortInfo? Author { get; init; }
	public UserShortInfo? Lead { get; init; }
	public Collection<UserShortInfo>? TeamUsers { get; init; }
	public Collection<UserShortInfo>? Clients { get; init; }
	public Collection<UserShortInfo>? Followers { get; init; }
	public Collection<string>? Tags { get; init; }
	public DateTime? Start { get; init; }
	public DateTime? End { get; init; }
	public bool? TeamAccess { get; init; }
	public ProjectEntityStatus? Status { get; init; }
	public Collection<string>? Quarter { get; init; }
	public Collection<string>? ChecklistIds { get; init; }
	public int? ParentId { get; init; }

	public override int GetHashCode()
	{
		var hashCodePart1 = HashCode.Combine(Id, ShortId, ProjectType, CreatedBy, CreatedAt, UpdatedAt, Summary, Description);

		var hashCodePart2 = HashCode.Combine(Author, Lead, Start, End, TeamAccess, Status, ParentId);

		var collectionHashCode = 0;

		foreach (var teamUser in TeamUsers ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, teamUser.GetHashCode());
		}

		foreach (var client in Clients ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, client.GetHashCode());
		}

		foreach (var follower in Followers ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, follower.GetHashCode());
		}

		foreach (var tag in Tags ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, tag.GetHashCode(StringComparison.InvariantCulture));
		}

		foreach (var quarter in Quarter ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, quarter.GetHashCode(StringComparison.InvariantCulture));
		}

		foreach (var checklistId in ChecklistIds ?? [])
		{
			collectionHashCode = HashCode.Combine(collectionHashCode, checklistId.GetHashCode(StringComparison.InvariantCulture));
		}

		return HashCode.Combine(hashCodePart1, hashCodePart2, collectionHashCode);
	}
}
