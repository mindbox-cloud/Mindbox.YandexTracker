using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.ObjectModel;

namespace Mindbox.YandexTracker;

public static class DtoExtension
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

	public static Resolution ToResolution(this FieldInfo value) => value.Id switch
	{
		"fixed" => new Resolution { Key = value.Id, Name = "Решен" },
		"wontFix" => new Resolution { Key = value.Id, Name = "Не будет исправлено" },
		"cantReproduce" => new Resolution { Key = value.Id, Name = "Не воспроизводится" },
		"duplicate" => new Resolution { Key = value.Id, Name = "Дубликат" },
		"later" => new Resolution { Key = value.Id, Name = "Позже" },
		"overfulfilled" => new Resolution { Key = value.Id, Name = "Перевыполнено" },
		"successful" => new Resolution { Key = value.Id, Name = "Успешно" },
		"dontDo" => new Resolution { Key = value.Id, Name = "Не делаем" },
		_ => throw new ArgumentOutOfRangeException(value.Id)
	};

	public static IssueType ToIssueType(this FieldInfo value) => value.Key switch
	{
		"bug" => new IssueType { Name = "Ошибка", Key = value.Key },
		"task" => new IssueType { Name = "Задача", Key = value.Key },
		"newFeature" => new IssueType { Name = "Новая возможность", Key = value.Key },
		"improvement" => new IssueType { Name = "Улучшение", Key = value.Key },
		"refactoring" => new IssueType { Name = "Рефакторинг", Key = value.Key },
		"epic" => new IssueType { Name = "Epic", Key = value.Key },
		"story" => new IssueType { Name = "Story", Key = value.Key },
		"changeRequest" => new IssueType { Name = "Change Request", Key = value.Key },
		"incident" => new IssueType { Name = "Инцидент", Key = value.Key },
		"serviceRequest" => new IssueType { Name = "Запрос на обслуживание", Key = value.Key },
		"release" => new IssueType { Name = "Релиз", Key = value.Key },
		"project" => new IssueType { Name = "Проект", Key = value.Key },
		"leave" => new IssueType { Name = "Отсутствие", Key = value.Key },
		"businessTrip" => new IssueType { Name = "Командировка", Key = value.Key },
		"changes" => new IssueType { Name = "Изменения", Key = value.Key },
		"request" => new IssueType { Name = "Запрос", Key = value.Key },
		"vacancy" => new IssueType { Name = "Вакансия", Key = value.Key },
		"applicant" => new IssueType { Name = "Кандидат", Key = value.Key },
		"goal" => new IssueType { Name = "Цель", Key = value.Key },
		"milestone" => new IssueType { Name = "Веха", Key = value.Key },
		_ => throw new ArgumentOutOfRangeException(value.Key)
	};

	public static Priority ToPriority(this FieldInfo value) => value.Key switch
	{
		"trivial" => Priority.Trivial,
		"minor" => Priority.Minor,
		"normal" => Priority.Normal,
		"critical" => Priority.Critical,
		"blocker" => Priority.Blocker,
		_ => throw new ArgumentOutOfRangeException(value.Key)
	};


	public static IssueTypeConfig ToIssueTypeConfig(this IssueTypeConfigDto value)
	{
#pragma warning disable CA1305
		return new IssueTypeConfig
		{
			IssueType = value.IssueType.ToIssueType(),
			Workflow = value.Workflow.Id.ToString(),
			Resolutions = new Collection<Resolution>(value.Resolutions!.Select(dto => dto.ToResolution()).ToList())
		};
#pragma warning restore CA1305
	}

	public static IssueStatus ToIssueStatus(this FieldInfo value) => value.Key switch
	{
		"open" => new IssueStatus { Name = "Открыт", Type = IssueStatusType.New, Key = value.Key },
		"needInfo" => new IssueStatus { Name = "Требуется информация", Type = IssueStatusType.Paused, Key = value.Key },
		"inProgress" => new IssueStatus { Name = "В работе", Type = IssueStatusType.InProgress, Key = value.Key },
		"testing" => new IssueStatus { Name = "Тестируется", Type = IssueStatusType.InProgress, Key = value.Key },
		"tested" => new IssueStatus { Name = "Протестировано", Type = IssueStatusType.Paused, Key = value.Key },
		"inReview" => new IssueStatus { Name = "Ревью", Type = IssueStatusType.Paused, Key = value.Key },
		"resolved" => new IssueStatus { Name = "Решен", Type = IssueStatusType.Done, Key = value.Key },
		"closed" => new IssueStatus { Name = "Закрыт", Type = IssueStatusType.Done, Key = value.Key },
		"rc" => new IssueStatus { Name = "Готов к релизу", Type = IssueStatusType.Paused, Key = value.Key },
		"backlog" => new IssueStatus { Name = "Бэклог", Type = IssueStatusType.New, Key = value.Key },
		"selectedForDev" => new IssueStatus { Name = "Будем делать", Type = IssueStatusType.New, Key = value.Key },
		"readyForTest" => new IssueStatus { Name = "Можно тестировать", Type = IssueStatusType.Paused, Key = value.Key },
		"needAcceptance" => new IssueStatus { Name = "Ждем подтверждения", Type = IssueStatusType.Paused, Key = value.Key },
		"cancelled" => new IssueStatus { Name = "Отменено", Type = IssueStatusType.Canceled, Key = value.Key },
		"confirmed" => new IssueStatus { Name = "Подтверждён", Type = IssueStatusType.Paused, Key = value.Key },
		"needEstimate" => new IssueStatus { Name = "Оценка задачи", Type = IssueStatusType.Paused, Key = value.Key },
		"demoToCustomer" => new IssueStatus { Name = "Демонстрация заказчику", Type = IssueStatusType.Paused, Key = value.Key },
		"firstSupportLine" => new IssueStatus
		{
			Name = "Первая линия поддержки",
			Type = IssueStatusType.InProgress,
			Key = value.Key
		},
		"secondSupportLine" => new IssueStatus
		{
			Name = "Вторая линия поддержки",
			Type = IssueStatusType.InProgress,
			Key = value.Key
		},
		"new" => new IssueStatus { Name = "Новый", Type = IssueStatusType.New, Key = value.Key },
		"documentsPrepared" => new IssueStatus
		{
			Name = "Документы подготовлены",
			Type = IssueStatusType.Paused,
			Key = value.Key
		},
		"onHold" => new IssueStatus { Name = "Приостановлено", Type = IssueStatusType.Paused, Key = value.Key },
		"resultAcceptance" => new IssueStatus
		{
			Name = "Согласование результата",
			Type = IssueStatusType.Paused,
			Key = value.Key
		},
		"newGoal" => new IssueStatus { Name = "Новая цель", Type = IssueStatusType.New, Key = value.Key },
		"asPlanned" => new IssueStatus { Name = "По плану", Type = IssueStatusType.InProgress, Key = value.Key },
		"withRisks" => new IssueStatus { Name = "Есть риски", Type = IssueStatusType.Paused, Key = value.Key },
		"achieved" => new IssueStatus { Name = "Достигнута", Type = IssueStatusType.Done, Key = value.Key },
		"blockedGoal" => new IssueStatus { Name = "Цель заблокирована", Type = IssueStatusType.Paused, Key = value.Key },
		"grommed" => new IssueStatus { Name = "Прогрумлено", Type = IssueStatusType.Paused, Key = value.Key },
		_ => throw new ArgumentOutOfRangeException(value.Id)
	};


	public static Queue ToQueue(this GetQueuesResponse value)
	{
		return new Queue
		{
			Id = value.Id,
			Key = value.Key,
			Name = value.Name,
			Lead = value.Lead.ToUserInfo(),
			Description = value.Description,
			AssignAuto = value.AssignAuto,
			DefaultType = value.DefaultType.ToIssueType(),
			DefaultPriority = value.DefaultPriority.ToPriority(),
			TeamUsers = new Collection<UserInfo>(value.TeamUsers.Select(dto => dto.ToUserInfo()).ToList()),
			IssueTypes = new Collection<IssueType>(value.IssueTypes.Select(dto => dto.ToIssueType()).ToList()),
			IssueTypesConfig = new Collection<IssueTypeConfig>(
				value.IssueTypesConfigDto.Select(dto => dto.ToIssueTypeConfig()).ToList()),
			Workflows = new Collection<IssueType>(value.Workflows?.Fields.Select(field => field.ToIssueType()).ToList()!),
			DenyVoting = value.DenyVoting
		};
	}

	public static UserInfo ToUserInfo(this FieldInfo value)
	{
		return new UserInfo { Id = value.Key! };
	}

	public static Issue ToIssue(this GetIssueResponse value)
	{
		return new Issue
		{
			Key = value.Key,
			LastCommentUpdatedAt = value.LastCommentUpdatedAt,
			Summary = value.Summary,
			Parent = value.Parent?.Key,
			UpdatedBy = value.UpdatedBy.ToUserInfo(),
			Description = value.Description,
			Type = value.Type.ToIssueType(),
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
			Status = value.Status.ToIssueStatus(),
			PreviousStatus = value.PreviousStatus?.ToIssueStatus(),
			IsFavorite = value.IsFavorite
		};
	}

	public static Issue ToIssue(this CreateIssueResponse value)
	{
		return new Issue
		{
			Key = value.Key,
			LastCommentUpdatedAt = value.LastCommentUpdatedAt,
			Summary = value.Summary,
			Parent = value.Parent?.Key,
			UpdatedBy = value.UpdatedBy.ToUserInfo(),
			Description = value.Description,
			Type = value.Type.ToIssueType(),
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
			Status = value.Status.ToIssueStatus(),
			PreviousStatus = value.PreviousStatus?.ToIssueStatus(),
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
				Summary = (string)summary!,
				Description = (string)description!,
				Author = ((FieldInfo)author!)?.ToUserInfo(),
				Lead = ((FieldInfo)lead!)?.ToUserInfo(),
				TeamUsers = new Collection<UserInfo>(((List<FieldInfo>)teamUsers!)?.Select(dto => dto.ToUserInfo()).ToList()!),
				Clients = new Collection<UserInfo>(((List<FieldInfo>)clients!)?.Select(dto => dto.ToUserInfo()).ToList()!),
				Followers = new Collection<UserInfo>(((List<FieldInfo>)followers!)?.Select(dto => dto.ToUserInfo()).ToList()!),
				Tags = new Collection<string>(((List<string>)tags!)),
				Start = (DateTime)start!,
				End = (DateTime)end!,
				TeamAccess = (bool)teamAccess!,
				Status = ((string)status!)?.ToProjectStatus(),
				Quarter = new Collection<string>(((List<string>)quarter!)),
				ChecklistIds = new Collection<string>(((List<FieldInfo>)checklist!)?.Select(dto => dto.Id).ToList()!)
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
