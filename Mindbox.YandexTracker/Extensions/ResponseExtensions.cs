using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

internal static class ResponseExtensions
{
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
		IReadOnlyDictionary<string, IssueType> issueTypeInfos,
		IReadOnlyDictionary<string, Resolution> resolutionInfos)
	{
		return new IssueTypeConfig
		{
			IssueType = value.IssueType.ToIssueType(issueTypeInfos),
			Workflow = value.Workflow.Id,
			Resolutions = new Collection<Resolution>(value.Resolutions
				.Select(dto => dto.ToResolution(resolutionInfos))
				.ToList())
		};
	}

	public static Resolution ToResolution(
		this FieldInfo value,
		IReadOnlyDictionary<string, Resolution> resolutionInfos)
	{
		return !resolutionInfos.TryGetValue(value.Key!, out var resolution)
			? throw new ArgumentException($"Resolution with key = {value.Key} not found")
			: resolution;
	}

	public static IssueType ToIssueType(
		this FieldInfo value,
		IReadOnlyDictionary<string, IssueType> issueTypes)
	{
		return !issueTypes.TryGetValue(value.Key!, out var issueType)
			? throw new ArgumentException($"IssueType with key = {value.Key} not found")
			: issueType;
	}

	public static IssueStatus ToIssueStatus(
		this FieldInfo value,
		IReadOnlyDictionary<string, IssueStatus> issueStatuses)
	{
		return !issueStatuses.TryGetValue(value.Key!, out var issueStatusInfo)
			? throw new ArgumentException($"IssueStatus with key = {value.Key} not found")
			: issueStatusInfo;
	}

	public static Queue ToQueue(
		this CreateQueueResponse value,
		IReadOnlyDictionary<string, IssueType> issueTypeInfos)
	{
		return new Queue
		{
			DefaultType = value.DefaultType.ToIssueType(issueTypeInfos),
			Id = value.Id,
			Key = value.Key,
			Lead = value.Lead.ToUserInfo(),
			Name = value.Name,
			AssignAuto = value.AssignAuto,
			DefaultPriority = value.DefaultPriority.ToPriority()
		};
	}

	public static Queue ToQueue(
		this GetQueuesResponse value,
		IReadOnlyDictionary<string, IssueType> issueTypeInfos,
		IReadOnlyDictionary<string, Resolution> resolutionInfos)
	{
		var workflows = value.Workflows.Count > 0
			? (value.Workflows
				.Select(x => x.Value)
				.Select(value => value.Select(field => field.ToIssueType(issueTypeInfos)))
				.Aggregate((x, y) => x.Concat(y))
				.ToList())
			: [];

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
			TeamUsers = new Collection<UserShortInfo>(value.TeamUsers.Select(dto => dto.ToUserInfo()).ToList()),
			IssueTypes = new Collection<IssueType>(value.IssueTypes.Select(dto => dto.ToIssueType(issueTypeInfos)).ToList()),
			IssueTypesConfig = new Collection<IssueTypeConfig>(
				value.IssueTypesConfig.Select(dto => dto.ToIssueTypeConfig(issueTypeInfos, resolutionInfos)).ToList()),
			Workflows = new Collection<IssueType>(workflows),
			DenyVoting = value.DenyVoting
		};
	}

	public static UserShortInfo ToUserInfo(this FieldInfo value)
	{
		ArgumentNullException.ThrowIfNull(value, nameof(value));

		if (!long.TryParse(value.Id, out var id))
			throw new ArgumentException($"Invalid id = {value.Id}", nameof(value));

		return new UserShortInfo { Id = id, Display = value.Display };
	}

	public static UserDetailedInfo ToUserDetailedInfo(this UserDetailedInfoDto dto)
	{
		return new UserDetailedInfo
		{
			Display = dto.Display,
			Email = dto.Email,
			Id = dto.Id,
			Login = dto.Login,
			FirstName = dto.FirstName,
			LastName = dto.LastName,
			PassportUid = dto.PassportUid,
			TrackerUid = dto.TrackerUid,
			Dismissed = dto.Dismissed,
			External = dto.External,
			DisableNotifications = dto.DisableNotifications,
			HasLicense = dto.HasLicense,
			UseNewFilters = dto.UseNewFilters,
			LastLoginDateUtc = dto.LastLoginDateUtc,
			FirstLoginDateUtc = dto.FirstLoginDateUtc,
			WelcomeMailSent = dto.WelcomeMailSent
		};
	}

	public static Issue ToIssue(
		this GetIssueResponse value,
		IReadOnlyDictionary<string, IssueType> issueTypeInfos,
		IReadOnlyDictionary<string, IssueStatus> issueStatusInfos)
	{
		return new Issue
		{
			Key = value.Key,
			LastCommentUpdatedAtUtc = value.LastCommentUpdatedAt,
			Summary = value.Summary,
			ParentKey = value.Parent?.Key,
			UpdatedBy = value.UpdatedBy.ToUserInfo(),
			Description = value.Description,
			Type = value.Type.ToIssueType(issueTypeInfos),
			Priority = value.Priority.ToPriority(),
			CreatedAtUtc = value.CreatedAt,
			Aliases = value.Aliases,
			Sprints = new Collection<string>(value.Sprints.Select(sprint => sprint.Id).ToList()),
			Followers = new Collection<UserShortInfo>(value.Followers.Select(follower => follower.ToUserInfo()).ToList()),
			CreatedBy = value.CreatedBy.ToUserInfo(),
			Votes = value.Votes,
			Assignee = value.Assignee?.ToUserInfo(),
			Project = value.Project?.Id,
			QueueKey = value.Queue.Key!,
			UpdatedAt = value.UpdatedAt,
			Status = value.Status.ToIssueStatus(issueStatusInfos),
			PreviousStatus = value.PreviousStatus?.ToIssueStatus(issueStatusInfos),
			IsFavorite = value.IsFavorite,
			Start = value.Start,
			Tags = value.Tags,
			CustomFields = value.Fields
		};
	}

	public static Issue ToIssue(
		this CreateIssueResponse value,
		IReadOnlyDictionary<string, IssueType> issueTypeInfos,
		IReadOnlyDictionary<string, IssueStatus> issueStatusInfos)
	{
		return new Issue
		{
			Key = value.Key,
			LastCommentUpdatedAtUtc = value.LastCommentUpdatedAt,
			Summary = value.Summary,
			ParentKey = value.Parent?.Key,
			UpdatedBy = value.UpdatedBy.ToUserInfo(),
			Description = value.Description,
			Type = value.Type.ToIssueType(issueTypeInfos),
			Priority = value.Priority.ToPriority(),
			CreatedAtUtc = value.CreatedAt,
			Aliases = value.Aliases,
			Sprints = new Collection<string>(value.Sprints.Select(sprint => sprint.Id).ToList()),
			Followers = new Collection<UserShortInfo>(value.Followers.Select(follower => follower.ToUserInfo()).ToList()),
			CreatedBy = value.CreatedBy.ToUserInfo(),
			Votes = value.Votes,
			Assignee = value.Assignee?.ToUserInfo(),
			Project = value.Project?.Id,
			QueueKey = value.Queue.Key!,
			UpdatedAt = value.UpdatedAt,
			Status = value.Status.ToIssueStatus(issueStatusInfos),
			PreviousStatus = value.PreviousStatus?.ToIssueStatus(issueStatusInfos),
			IsFavorite = value.IsFavorite,
			CustomFields = value.CustomFields
		};
	}

	public static Component ToComponent(this GetComponentResponse value)
	{
		return new Component
		{
			Id = value.Id,
			Name = value.Name,
			QueueKey = value.Queue.Key,
			Description = value.Description,
			AssignAuto = value.AssignAuto,
			Lead = value.Lead?.ToUserInfo()
		};
	}

	public static Component ToComponent(this CreateComponentResponse value)
	{
		return new Component
		{
			AssignAuto = value.AssignAuto,
			Name = value.Name,
			Id = value.Id,
			QueueKey = value.Queue?.Key
		};
	}

	public static Attachment ToAttachment(this GetAttachmentResponse value)
	{
		return new Attachment
		{
			Id = value.Id,
			Name = value.Name,
			ContentUrl = value.Content,
			ThumbnailUrl = value.Thumbnail,
			CreatedBy = value.CreatedBy.ToUserInfo(),
			CreatedAtUtc = value.CreatedAt,
			MimeType = value.Mimetype,
			SizeBytes = value.Size,
			Metadata = value.Metadata is not null
				? new AttachmentMetadata { GeometricSize = value.Metadata.Size }
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
			UpdatedBy = value.UpdatedBy.ToUserInfo(),
			CreatedAtUtc = value.CreatedAt,
			UpdatedAtUtc = value.UpdatedAt,
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
			CreatedAtUtc = value.CreatedAt,
			UpdatedAtUtc = value.UpdatedAt,
			Attachments = [],
			CommentType = value.Type,
			TransportType = value.TransportType
		};
	}

	public static CategoryShortInfo ToCategory(this GetFieldCategoriesResponse value)
	{
		return new CategoryShortInfo
		{
			Id = value.Id,
			Name = value.Name
		};
	}

	public static QueueLocalField ToQueueLocalField(this CreateQueueLocalFieldResponse value)
	{
		return new QueueLocalField
		{
			Id = value.Id,
			Description = value.Description,
			Readonly = value.Readonly,
			CategoryId = value.Category.Id,
			SerialNumberInFieldsOrganization = value.Order,
			FieldName = new QueueLocalFieldName // возвращается только поле на русском
			{
				Ru = value.Name,
				En = string.Empty
			},
			FieldType = value.Schema.ToQueueLocalFieldType()
		};
	}

	public static QueueLocalFieldType ToQueueLocalFieldType(this SchemaInfoDto value) => value.Type switch
	{
		"uri" => QueueLocalFieldType.UriFieldType,
		"user" => QueueLocalFieldType.UserFieldType,
		"integer" => QueueLocalFieldType.IntegerFieldType,
		"float" => QueueLocalFieldType.FloatFieldType,
		"date" => QueueLocalFieldType.DateFieldType,
		"datetime" => QueueLocalFieldType.DateTimeFieldType,
		"string" => QueueLocalFieldType.StringFieldType,
		"text" => QueueLocalFieldType.TextFieldType,
		_ => throw new ArgumentOutOfRangeException(value.Type)
	};

	public static Attachment ToAttachment(this CreateAttachmentResponse value)
	{
		return new Attachment
		{
			Id = value.Id,
			Name = value.Name,
			ContentUrl = value.Content,
			ThumbnailUrl = value.Thumbnail,
			CreatedBy = value.CreatedBy.ToUserInfo(),
			CreatedAtUtc = value.CreatedAt,
			MimeType = value.Mimetype,
			SizeBytes = value.Size,
			Metadata = value.Metadata is not null
				? new AttachmentMetadata { GeometricSize = value.Metadata.Size }
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
			projects.Add(new Project
			{
				Id = projectValue.Id,
		        ShortId = projectValue.ShortId,
		        ProjectType = projectValue.ProjectType,
		        CreatedBy = projectValue.CreatedBy.ToUserInfo(),
		        CreatedAtUtc = projectValue.CreatedAt,
		        UpdatedAtUtc = projectValue.UpdatedAt,
		        Summary = projectValue.Fields.TryGetValue("summary", out var summaryElement)
			        ? summaryElement.GetString()
			        : null,
				Description = projectValue.Fields.TryGetValue("description", out var descriptionElement)
					? descriptionElement.GetString()
					: null,
		        Author = projectValue.Fields.TryGetValue("author", out var authorElement)
			        ? authorElement.ToUserShortInfo()
			        : null,
		        Lead = projectValue.Fields.TryGetValue("lead", out var leadElement)
			        ? leadElement.ToUserShortInfo()
			        : null,
				TeamUsers = projectValue.Fields.TryGetValue("teamUsers", out var teamUsersElement)
					? teamUsersElement.ToUserCollection()
					: null,
		        Clients = projectValue.Fields.TryGetValue("clients", out var clientsElement)
			        ? clientsElement.ToUserCollection()
			        : null,
		        Followers = projectValue.Fields.TryGetValue("followers", out var followersElement)
			        ? followersElement.ToUserCollection()
			        : null,
		        Tags = projectValue.Fields.TryGetValue("tags", out var tagsElement)
			        ? tagsElement.ToStringCollection()
			        : null,
		        StartUtc = projectValue.Fields.TryGetValue("start", out var startUtcElement)
			        ? startUtcElement.GetDateTime()
			        : null,
		        EndUtc = projectValue.Fields.TryGetValue("end", out var endUtcElement)
			        ? endUtcElement.GetDateTime()
			        : null,
		        TeamAccess = projectValue.Fields.TryGetValue("teamAccess", out var teamAccessElement)
			        ? teamAccessElement.GetBoolean()
			        : null,
		        Status = projectValue.Fields.TryGetValue("entityStatus", out var statusElement)
			        ? statusElement.ToProjectEntityStatus()
			        : null,
		        Quarter = projectValue.Fields.TryGetValue("quarter", out var quarterElement)
			        ? quarterElement.ToStringCollection()
			        : null,
		        ChecklistIds = projectValue.Fields.TryGetValue("checklistItems", out var checklistIdsElement)
			        ? checklistIdsElement.ToStringCollection()
			        : null,
		        ParentId = projectValue.Fields.TryGetValue("parentEntity", out var parentIdElement)
			        ? parentIdElement.GetInt32()
			        : null,
		        IssueQueueKeys = projectValue.Fields.TryGetValue("issueQueues", out var issueQueues)
			        ? new Collection<string>(issueQueues.ToFieldInfoCollection().Select(x => x.Key!).ToList())
			        : null,
			});
		}

		return projects;
	}

	public static Project ToProject(this CreateProjectResponse value)
	{
		 return new Project
		{
	        Id = value.Id,
	        ShortId = value.ShortId,
	        ProjectType = value.ProjectEntityType,
	        CreatedBy = value.CreatedBy.ToUserInfo(),
	        CreatedAtUtc = value.CreatedAt,
	        UpdatedAtUtc = value.UpdatedAt,
	        Summary = value.Fields.TryGetValue("summary", out var summaryElement)
		        ? summaryElement.GetString()
		        : null,
			Description = value.Fields.TryGetValue("description", out var descriptionElement)
				? descriptionElement.GetString()
				: null,
	        Author = value.Fields.TryGetValue("author", out var authorElement)
		        ? authorElement.ToUserShortInfo()
		        : null,
	        Lead = value.Fields.TryGetValue("lead", out var leadElement)
		        ? leadElement.ToUserShortInfo()
		        : null,
			TeamUsers = value.Fields.TryGetValue("teamUsers", out var teamUsersElement)
				? teamUsersElement.ToUserCollection()
				: null,
	        Clients = value.Fields.TryGetValue("clients", out var clientsElement)
		        ? clientsElement.ToUserCollection()
		        : null,
	        Followers = value.Fields.TryGetValue("followers", out var followersElement)
		        ? followersElement.ToUserCollection()
		        : null,
	        Tags = value.Fields.TryGetValue("tags", out var tagsElement)
		        ? tagsElement.ToStringCollection()
		        : null,
	        StartUtc = value.Fields.TryGetValue("start", out var startUtcElement)
		        ? startUtcElement.GetDateTime()
		        : null,
	        EndUtc = value.Fields.TryGetValue("end", out var endUtcElement)
		        ? endUtcElement.GetDateTime()
		        : null,
	        TeamAccess = value.Fields.TryGetValue("teamAccess", out var teamAccessElement)
		        ? teamAccessElement.GetBoolean()
		        : null,
	        Status = value.Fields.TryGetValue("status", out var statusElement)
		        ? statusElement.ToProjectEntityStatus()
		        : null,
	        Quarter = value.Fields.TryGetValue("quarter", out var quarterElement)
		        ? quarterElement.ToStringCollection()
		        : null,
	        ChecklistIds = value.Fields.TryGetValue("checklistItems", out var checklistIdsElement)
		        ? checklistIdsElement.ToStringCollection()
		        : null,
	        ParentId = value.Fields.TryGetValue("parentEntity", out var parentIdElement)
		        ? parentIdElement.GetInt32()
		        : null
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
			OptionsProvider = value.OptionsProvider is not null
				? new OptionsProviderInfo
				{
					Type = value.OptionsProvider.Type,
					Values = value.OptionsProvider.Values
				}
				: null,
			QueryProvider = value.QueryProvider?.Type,
			SuggestProvider = value.SuggestProvider?.Type,
			Order = value.Order,
			CategoryId = value.Category.Id,
			Schema = value.Schema.Type
		};
	}
}
