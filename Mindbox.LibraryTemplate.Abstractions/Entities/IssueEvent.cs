using System;

namespace Mindbox.YandexTracker;

public sealed class IssueEvent
{
	internal IssueEvent(IssueEventDto eventDto)
	{
		ArgumentNullException.ThrowIfNull(eventDto);

		Issue = eventDto.Issue?.ToIssue()!;
		CreatedDateTimeUtc = eventDto.CreatedAt;
		EventType = Enum.TryParse<IssueEventType>(eventDto.Event, true, out var eventType)
			? eventType
			: IssueEventType.Unknown;
	}

	public IssueEventType EventType { get; set; }
	public DateTime CreatedDateTimeUtc { get; set; }
	public Issue Issue { get; set; }
}