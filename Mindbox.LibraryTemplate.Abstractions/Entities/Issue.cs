using System;

namespace Mindbox.YandexTracker;

public sealed class Issue
{
	public required string Title { get; set; }
	public required string Body { get; set; }
	public required string Url { get; set; }
	public IssueState State { get; set; }
	public IssueType? Type { get; set; }
	public IssueEstimate? Estimate { get; set; }
	public DateTime CreationDateTimeUtc { get; set; }
	public DateTime? ClosingDateTimeUtc { get; set; }
#pragma warning disable CA1819 // Properties should not return arrays
	public required Account Assignee { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
#pragma warning disable CA1819 // Properties should not return arrays
	public required string[] RawLabels { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
	public bool IsCustom { get; set; }
	public bool IsOvertime { get; set; }
	public bool IsPullRequest { get; set; }
	public DateTime LastUpdateDateTimeUtc { get; set; }
	public int Number { get; set; }
}