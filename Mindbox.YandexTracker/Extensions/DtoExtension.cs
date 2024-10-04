using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

internal static class DtoExtension
{
	public static string ToQueryString<T>(this T value)
		where T : Enum
	{
		var enumValues = Enum.GetValues(typeof(T))
			.Cast<T>()
			.Where(enumValue => value.HasFlag(enumValue!))
			.Select(enumValue => enumValue!.ToString())
			.Skip(1); // skip None

		return string.Join(",", enumValues);
	}

	public static Resolution ToResolution(
		this FieldInfo value,
		IReadOnlyDictionary<string, GetResolutionResponse> resolutionInfos)
	{
		if (!resolutionInfos.TryGetValue(value.Key!, out var resolutionInfo))
		{
			throw new ArgumentException($"Resolution with key = {value.Key} not found");
		}

		return new Resolution
		{
			Key = resolutionInfo.Key,
			Name = resolutionInfo.Name,
			Description = resolutionInfo.Description
		};
	}

	public static IssueType ToIssueType(
		this FieldInfo value,
		IReadOnlyDictionary<string, GetIssueTypeResponse> issueTypeInfos) 
	{
		if (!issueTypeInfos.TryGetValue(value.Key!, out var issueTypeInfo))
		{
			throw new ArgumentException($"IssueType with key = {value.Key} not found");
		}

		return new IssueType
		{
			Key = issueTypeInfo.Key,
			Name = issueTypeInfo.Name,
			Description = issueTypeInfo.Description	
		};
	}

	public static Priority ToPriority(this FieldInfo value) => value.Key switch
	{
		"trivial" => Priority.Trivial,
		"minor" => Priority.Minor,
		"normal" => Priority.Normal,
		"critical" => Priority.Critical,
		"blocker" => Priority.Blocker,
		_ => throw new ArgumentOutOfRangeException(value.Key)
	};


	public static IssueTypeConfig ToIssueTypeConfig(
		this IssueTypeConfigDto value,
		IReadOnlyDictionary<string, GetIssueTypeResponse> issueTypeInfos,
		IReadOnlyDictionary<string, GetResolutionResponse> resolutionInfos)
	{
		return new IssueTypeConfig
		{
			IssueType = value.IssueType.ToIssueType(issueTypeInfos),
			Workflow = value.Workflow.Id.ToString(),
			Resolutions = new Collection<Resolution>(value.Resolutions!
				.Select(dto => dto.ToResolution(resolutionInfos))
				.ToList())
		};
	}

	public static IssueStatus ToIssueStatus(
		this FieldInfo value,
		IReadOnlyDictionary<string, GetIssueStatusResponse> issueStatusInfos)
	{
		if (!issueStatusInfos.TryGetValue(value.Key!, out var issueStatusInfo))
		{
			throw new ArgumentException($"IssueStatus with key = {value.Key} not found");
		}

		return new IssueStatus
		{
			Key = issueStatusInfo.Key,
			Name = issueStatusInfo.Name,
			Type = issueStatusInfo.Type,
			Description = issueStatusInfo.Description
		};
	}


	public static Queue ToQueue(
		this GetQueuesResponse value,
		IReadOnlyDictionary<string, GetIssueTypeResponse> issueTypeInfos,
		IReadOnlyDictionary<string, GetResolutionResponse> resolutionInfos)
	{
		return new Queue
		{
			Id = value.Id,
			Key = value.Key,
			Name = value.Name,
			Lead = value.Lead.ToUserInfo(),
			Description = value.Description,
			AssignAuto = value.AssignAuto,
			DefaultType = value.DefaultType.ToIssueType(issueTypeInfos),
			DefaultPriority = value.DefaultPriority.ToPriority(),
			TeamUsers = new Collection<UserInfo>(value.TeamUsers.Select(dto => dto.ToUserInfo()).ToList()),
			IssueTypes = new Collection<IssueType>(value.IssueTypes.Select(dto => dto.ToIssueType(issueTypeInfos)).ToList()),
			IssueTypesConfig = new Collection<IssueTypeConfig>(
				value.IssueTypesConfigDto.Select(dto => dto.ToIssueTypeConfig(issueTypeInfos, resolutionInfos)).ToList()),
			Workflows = new Collection<IssueType>(
				value.Workflows?.Fields.Select(field => field.ToIssueType(issueTypeInfos)).ToList()!),
			DenyVoting = value.DenyVoting
		};
	}

	public static UserInfo ToUserInfo(this FieldInfo value)
	{
		return new UserInfo { Id = value.Key! };
	}

	public static Issue ToIssue(
		this GetIssueResponse value,
		IReadOnlyDictionary<string, GetIssueTypeResponse> issueTypeInfos,
		IReadOnlyDictionary<string, GetIssueStatusResponse> issueStatusInfos)
	{
		return new Issue
		{
			Key = value.Key,
			LastCommentUpdatedAt = value.LastCommentUpdatedAt,
			Summary = value.Summary,
			Parent = value.Parent?.Key,
			UpdatedBy = value.UpdatedBy.ToUserInfo(),
			Description = value.Description,
			Type = value.Type.ToIssueType(issueTypeInfos),
			Priority = value.Priority.ToPriority(),
			CreatedAt = value.CreatedAt,
			Aliases = value.Aliases,
			Sprints = new Collection<string>(value.Sprints.Select(sprint => sprint.Id).ToList()),
			Followers = new Collection<UserInfo>(value.Followers.Select(follower => follower.ToUserInfo()).ToList()),
			CreatedBy = value.CreatedBy.ToUserInfo(),
			Votes = value.Votes,
			Assignee = value.Assignee!.ToUserInfo(),
			Project = value.Project?.Id,
			Queue = value.Queue.Key!,
			UpdatedAt = value.UpdatedAt,
			Status = value.Status.ToIssueStatus(issueStatusInfos),
			PreviousStatus = value.PreviousStatus?.ToIssueStatus(issueStatusInfos),
			IsFavorite = value.IsFavorite
		};
	}

	public static Issue ToIssue(
		this CreateIssueResponse value,
		IReadOnlyDictionary<string, GetIssueTypeResponse> issueTypeInfos,
		IReadOnlyDictionary<string, GetIssueStatusResponse> issueStatusInfos)
	{
		return new Issue
		{
			Key = value.Key,
			LastCommentUpdatedAt = value.LastCommentUpdatedAt,
			Summary = value.Summary,
			Parent = value.Parent?.Key,
			UpdatedBy = value.UpdatedBy.ToUserInfo(),
			Description = value.Description,
			Type = value.Type.ToIssueType(issueTypeInfos),
			Priority = value.Priority.ToPriority(),
			CreatedAt = value.CreatedAt,
			Aliases = value.Aliases,
			Sprints = new Collection<string>(value.Sprints.Select(sprint => sprint.Id).ToList()),
			Followers = new Collection<UserInfo>(value.Followers.Select(follower => follower.ToUserInfo()).ToList()),
			CreatedBy = value.CreatedBy.ToUserInfo(),
			Votes = value.Votes,
			Assignee = value.Assignee?.ToUserInfo(),
			Project = value.Project?.Id,
			Queue = value.Queue.Key!,
			UpdatedAt = value.UpdatedAt,
			Status = value.Status.ToIssueStatus(issueStatusInfos),
			PreviousStatus = value.PreviousStatus?.ToIssueStatus(issueStatusInfos),
			IsFavorite = value.IsFavorite
		};
	}

	public static Component ToComponent(this GetComponentResponse value)
	{
		return new Component
		{
			Id = value.Id,
			Name = value.Name,
			Queue = value.Queue.Key,
			Description = value.Description,
			AssignAuto = value.AssignAuto,
			Lead = value.Lead?.ToUserInfo()
		};
	}

	public static Attachment ToAttachment(this GetAttachmentResponse value)
	{
		return new Attachment
		{
			Id = value.Id,
			Name = value.Name,
			Content = value.Content,
			Thumbnail = value.Thumbnail,
			CreatedBy = value.CreatedBy.ToUserInfo(),
			CreatedAt = value.CreatedAt,
			MimeType = value.Mimetype,
			Size = value.Size,
			Metadata = value.Metadata is not null
				? new AttachmentData { Size = value.Metadata.Size }
				: null
		};
	}

	public static Comment ToComment(this GetCommentsResponse value)
	{
		return new Comment
		{
			Id = value.Id,
			Text = value.Text,
			CreatedBy = value.CreatedBy.ToUserInfo(),
			UpdatedBy = value.UpdatedBy?.ToUserInfo(),
			CreatedAt = value.CreatedAt,
			UpdatedAt = value.UpdatedAt,
			Attachments = new Collection<string>(value.Attachments.Select(dto => dto.Id).ToList()),
			CommentType = value.Type,
			TransportType = value.TransportType
		};
	}

	public static Comment ToComment(this CreateCommentResponse value)
	{
		return new Comment
		{
			Id = value.Id,
			Text = value.Text,
			CreatedBy = value.CreatedBy.ToUserInfo(),
			UpdatedBy = value.UpdatedBy.ToUserInfo(),
			CreatedAt = value.CreatedAt,
			UpdatedAt = value.UpdatedAt,
			Attachments = [],
			CommentType = value.Type,
			TransportType = value.TransportType
		};
	}

	public static Attachment ToAttachment(this CreateAttachmentResponse value)
	{
		return new Attachment
		{
			Id = value.Id,
			Name = value.Name,
			Content = value.Content,
			Thumbnail = value.Thumbnail,
			CreatedBy = value.CreatedBy.ToUserInfo(),
			CreatedAt = value.CreatedAt,
			MimeType = value.Mimetype,
			Size = value.Size,
			Metadata = value.Metadata is not null
				? new AttachmentData { Size = value.Metadata.Size }
				: null
		};
	}

	public static ProjectEntityStatus ToProjectStatus(this string value) => value switch
	{
		"draft" => ProjectEntityStatus.Draft,
		"in_progress" => ProjectEntityStatus.InProgress,
		"launched" => ProjectEntityStatus.Launched,
		"postponed" => ProjectEntityStatus.Postponed,
		"at_risk" => ProjectEntityStatus.AtRisk,
		"according_to_plan" => ProjectEntityStatus.AccordingToPlan,
		_ => throw new ArgumentOutOfRangeException(value)
	};


	public static IReadOnlyList<Project> ToProjects(this GetProjectsResponse value)
	{
		var projects = new List<Project>();

		foreach (var projectValue in value.Values)
		{
			projectValue.Fields.TryGetValue("summary", out var summary);
			projectValue.Fields.TryGetValue("description", out var description);
			projectValue.Fields.TryGetValue("author", out var author);
			projectValue.Fields.TryGetValue("lead", out var lead);
			projectValue.Fields.TryGetValue("teamUsers", out var teamUsers);
			projectValue.Fields.TryGetValue("clients", out var clients);
			projectValue.Fields.TryGetValue("followers", out var followers);
			projectValue.Fields.TryGetValue("tags", out var tags);
			projectValue.Fields.TryGetValue("start", out var start);
			projectValue.Fields.TryGetValue("end", out var end);
			projectValue.Fields.TryGetValue("teamAccess", out var teamAccess);
			projectValue.Fields.TryGetValue("status", out var status);
			projectValue.Fields.TryGetValue("quarter", out var quarter);
			projectValue.Fields.TryGetValue("checklistItems", out var checklist);

			projects.Add(new Project
			{
				Id = projectValue.Id,
				ProjectType = projectValue.ProjectType,
				CreatedBy = projectValue.CreatedBy.ToUserInfo(),
				CreatedAt = projectValue.CreatedAt,
				UpdatedAt = projectValue.UpdatedAt,
				Summary = (string?)summary,
				Description = (string?)description,
				Author = ((FieldInfo?)author)?.ToUserInfo(),
				Lead = ((FieldInfo?)lead)?.ToUserInfo(),
				TeamUsers = teamUsers is not null
					? new Collection<UserInfo>(((List<FieldInfo>)teamUsers).Select(dto => dto.ToUserInfo()).ToList())
					: null,
				Clients = clients is not null
					? new Collection<UserInfo>(((List<FieldInfo>)clients).Select(dto => dto.ToUserInfo()).ToList())
					: null,
				Followers = followers is not null
					? new Collection<UserInfo>(((List<FieldInfo>)followers).Select(dto => dto.ToUserInfo()).ToList())
					: null,
				Tags = tags is not null
					? new Collection<string>((List<string>)tags)
					: null,
				Start = (DateTime?)start,
				End = (DateTime?)end,
				TeamAccess = (bool?)teamAccess,
				Status = ((string?)status)?.ToProjectStatus(),
				Quarter = quarter is not null
					? new Collection<string>(((List<string>)quarter))
					: null,
				ChecklistIds = checklist is not null
					? new Collection<string>(((List<FieldInfo>)checklist).Select(dto => dto.Id).ToList())
					: null
			});
		}

		return projects;
	}

	public static Project ToProject(this CreateProjectResponse value)
	{
		return new Project
		{
			Id = value.Id,
			ProjectType = value.ProjectEntityType,
			CreatedBy = value.CreatedBy.ToUserInfo(),
			CreatedAt = value.CreatedAt,
			UpdatedAt = value.UpdatedAt
		};
	}

	public static IssueField ToIssueField(this GetIssueFieldsResponse value)
	{
		return new IssueField
		{
			Id = value.Id,
			Key = value.Key,
			Name = value.Name,
			Readonly = value.Readonly,
			Options = value.Options,
			Suggest = value.Suggest,
			OptionsProvider = new OptionsProviderInfo
			{
				Type = value.OptionsProvider.Type,
				Values = value.OptionsProvider.Values
			},
			QueryProvider = value.QueryProvider?.Type,
			SuggestProvider = value.SuggestProvider?.Type,
			Order = value.Order,
			CategoryId = value.Category.Id,
			Schema = value.Schema.Type
		};
	}
}
