using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace Mindbox.YandexTracker;

internal static class RequestExtensions
{
	public static CreateIssueRequest ToCreateIssueRequest(this Issue issue)
	{
		var fields = new Dictionary<string, JsonElement>(issue.CustomFields)
		{
			["aliases"] = JsonSerializer.SerializeToElement(issue.Aliases),
			["project"] = JsonSerializer.SerializeToElement(issue.Project)
		};

		return new CreateIssueRequest
		{
			Queue = issue.QueueKey,
			Start = issue.Start,
			Summary = issue.Summary,
			Assignee = issue.Assignee?.Id,
			Description = issue.Description,
			Fields = fields,
			Followers = new Collection<string>(issue.Followers.Select(follower => follower.Id).ToList()),
			Parent = issue.ParentKey,
			Priority = issue.Priority,
			Sprints = issue.Sprints,
			Type = issue.Type,
			Author = issue.Author?.Id,
			Tags = issue.Tags,
		};
	}

	public static CreateCommentRequest ToCreateCommentRequest(this Comment comment)
	{
		return new CreateCommentRequest
		{
			Text = comment.Text,
			AttachmentIds = comment.Attachments,
			Summonees = comment.Summonees,
			MaillistSummonees = comment.MaillistSummonees
		};
	}

	public static CreateQueueRequest ToCreateQueueRequest(this Queue queue)
	{
		return new CreateQueueRequest
		{
			DefaultType = queue.DefaultType.Key,
			Key = queue.Key,
			LeadId = queue.Lead.Id,
			Name = queue.Name,
			DefaultPriority = queue.DefaultPriority,
			IssueTypesConfig = new Collection<CreateIssueTypeConfigDto>(
				queue.IssueTypesConfig
					.Select(config => config.ToDto())
					.ToList())
		};
	}

	public static CreateIssueTypeConfigDto ToDto(this IssueTypeConfig config)
	{
		return new CreateIssueTypeConfigDto
		{
			IssueType = config.IssueType.Key,
			Workflow = config.Workflow,
			Resolutions = new Collection<string>(config.Resolutions.Select(resolution => resolution.Key).ToList())
		};
	}

	public static CreateProjectRequest ToCreateProjectRequest(this Project project)
	{
		return new CreateProjectRequest
		{
			Fields = project.ToProjectFieldsDto()
		};
	}

	public static CreateQueueLocalFieldRequest ToCreateQueueLocalFieldRequest(this QueueLocalField field)
	{
		return new CreateQueueLocalFieldRequest
		{
			Id = field.Id,
			Name = field.FieldName,
			CategoryId = field.CategoryId,
			Description = field.Description,
			Type = field.FieldType,
			OptionsProvider = field.OptionsProvider?.ToDto(),
			Readonly = field.Readonly,
			Container = field.Container,
			Order = field.SerialNumberInFieldsOrganization,
			Hidden = field.Hidden,
			Visible = field.Visible
		};
	}

	private static OptionsProviderInfoDto ToDto(this OptionsProviderInfo value)
	{
		return new OptionsProviderInfoDto
		{
			Type = value.Type,
			Values = value.Values
		};
	}

	public static Dictionary<string, object> ToDictionary(this IssuesFilter filter)
	{
		var dict = new Dictionary<string, object>();

		for (var i = 0; i < filter.GetType().GetProperties().Length; i++)
		{
			var prop = filter.GetType().GetProperties()[i];

			var value = prop.GetValue(filter, null);
			if (prop.GetValue(filter, null) != null)
			{
#pragma warning disable CA1308 // Поля запроса в Яндекс трекере чувствительны к регистру, поэтому нужно перевести поле в lowerCase
				dict.Add(prop.Name.ToLowerInvariant(), value!);
#pragma warning restore CA1308
			}
		}

		return dict;
	}

	public static GetProjectsRequest ToGetProjectsRequest(
		this Project project,
		string? input,
		string? orderBy,
		bool? orderAscending,
		bool? rootOnly)
	{
		return new GetProjectsRequest
		{
			Filter = project.ToProjectFieldsDto(),
			Input = input,
			OrderAscending = orderAscending,
			OrderBy = orderBy,
			RootOnly = rootOnly
		};
	}

	public static ProjectFieldsDto ToProjectFieldsDto(this Project project)
	{
		return new ProjectFieldsDto
		{
			Summary = project.Summary!,
			AuthorId = project.Author?.Id,
			Clients = project.Clients.Count > 0
					? new Collection<string>(project.Clients.Select(client => client.Id).ToList())
					: null,
			Description = project.Description,
			End = project.EndUtc,
			EntityStatus = project.Status,
			Followers = project.Followers.Count > 0
					? new Collection<string>(project.Followers.Select(client => client.Id).ToList())
					: null,
			LeadId = project.Lead?.Id,
			ParentEntityId = project.ParentId,
			Start = project.StartUtc,
			Tags = project.Tags,
			TeamAccess = project.TeamAccess,
			TeamUsers = project.TeamUsers.Count > 0
					? new Collection<string>(project.TeamUsers.Select(client => client.Id).ToList())
					: null
		};
	}
}
