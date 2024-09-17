using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed class IssueDto
{
	[DataMember(Name = "title")]
	public required string Title { get; set; }

	[DataMember(Name = "body")]
	public required string Body { get; set; }

	[DataMember(Name = "html_url")]
	public required string HtmlUrl { get; set; }

	[DataMember(Name = "state")]
	public IssueState State { get; set; }

	[DataMember(Name = "labels")]
	public required Collection<LabelDto> Labels { get; set; }

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; set; }

	[DataMember(Name = "closed_at")]
	public DateTime? ClosedAt { get; set; }

	[DataMember(Name = "assignee")]
	public required Account Assignee { get; set; }

	public required object PullRequest { get; set; }

	[DataMember(Name = "updated_at")]
	public DateTime UpdatedAt { get; set; }

	[DataMember(Name = "updatedBy")]
	public Account? UpdateBy { get; set; }

	[DataMember(Name = "number")]
	public int Number { get; set; }

	[DataMember(Name = "project")]
	public required string ProjectName { get; set; }

	[DataMember(Name = "queue")]
	public required string QueueName { get; set; }
}