using System;
using System.Linq;

namespace Mindbox.YandexTracker;

public static class Extensions
{
	public static string TrimAndMakeNullIfEmpty(this string? value)
	{
		return !string.IsNullOrWhiteSpace(value) ? value.Trim() : null!;
	}

	public static TOut Transform<TIn, TOut>(this TIn @this, Func<TIn, TOut> transformer)
	{
		ArgumentNullException.ThrowIfNull(transformer);

		return transformer(@this);
	}

	public static Issue ToIssue(this IssueDto issueDto)
	{
		ArgumentNullException.ThrowIfNull(issueDto);

		return new Issue
		{
			Title = issueDto.Title,
			Body = issueDto.Body,
			Url = issueDto.HtmlUrl,
			State = issueDto.State,
			Type = TryGetHighest([.. issueDto.Labels], GetType),
			Estimate = TryGetHighest([.. issueDto.Labels], GetEstimate),
			RawLabels = issueDto.Labels.Select(l => l.Name).ToArray(),
			Assignee = issueDto.Assignee,
			CreationDateTimeUtc = issueDto.CreatedAt,
			ClosingDateTimeUtc = issueDto.ClosedAt,
			IsCustom = issueDto.Labels.Any(l => l.Name == TagConstants.IssueIsCustomTag),
			IsOvertime = issueDto.Labels.Any(l => l.Name == TagConstants.IssueIsOvertimeTag),
			IsPullRequest = issueDto.PullRequest != null,
			LastUpdateDateTimeUtc = issueDto.UpdatedAt,
			Number = issueDto.Number
		};
	}

	private static IssueType? GetType(string name) => name switch
	{
		TagConstants.IssueTypeBugTag => IssueType.Bug,
		TagConstants.IssueTypePriorityBug => IssueType.PriorityBug,
		TagConstants.IssueTypeRoadMapTag => IssueType.RoadMap,
		TagConstants.IssueTypeSmallTag => IssueType.Small,
		TagConstants.IssueTypeTechnicalDebtTag => IssueType.TechnicalDebt,
		TagConstants.IssueTypeSlackTimeTag => IssueType.SlackTime,
		_ => null,
	};

	private static IssueEstimate? GetEstimate(string name) => name switch
	{
		TagConstants.IssueEstimateOneDayTag => IssueEstimate.OneDay,
		TagConstants.IssueEstimateTwoDaysTag => IssueEstimate.TwoDays,
		TagConstants.IssueEstimateThreeDaysTag => IssueEstimate.ThreeDays,
		TagConstants.IssueEstimateFiveDaysTag => IssueEstimate.FiveDays,
		_ => null,
	};

	private static TValue? TryGetHighest<TValue>(LabelDto[] labels, Func<string, TValue?> func) where TValue : struct
	{
		return labels
			.Select(label => func(label.Name))
			.Where(i => i != null)
			.OrderByDescending(i => i)
			.FirstOrDefault();
	}
}