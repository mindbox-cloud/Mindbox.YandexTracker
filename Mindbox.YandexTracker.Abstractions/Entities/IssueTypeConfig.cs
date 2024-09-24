using System;

namespace Mindbox.YandexTracker;

public class IssueTypeConfig
{
	public required IssueType IssueType { get; set; }
	public required string Workflow { get; set; }
#pragma warning disable CA1819
#pragma warning disable IDE0301
	public Resolution[] Resolutions { get; set; } = Array.Empty<Resolution>();
#pragma warning restore IDE0301
#pragma warning restore CA1819
}
