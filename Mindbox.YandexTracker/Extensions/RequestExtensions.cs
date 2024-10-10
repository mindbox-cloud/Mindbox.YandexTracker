using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Mindbox.YandexTracker;

internal static class RequestExtensions
{
	public static CreateIssueRequest ToCreateIssueRequest(this Issue issue)
	{
		return new CreateIssueRequest
		{
			Queue = issue.Queue,
			Summary = issue.Summary,
			Assignee = issue.Assignee?.Id,
			Description = issue.Description,
			Fields = issue.CustomFields,
			Followers = new Collection<string>(issue.Followers.Select(follower => follower.Id).ToList()),
			Parent = issue.Parent,
			Priority = issue.Priority,
			Sprints = issue.Sprints,
			Type = issue.Type,
			Author = issue.Author?.Id
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
			DefaultType = queue.DefaultType.ToString(),
			Key = queue.Key,
			LeadId = queue.Lead.Id,
			Name = queue.Name,
			DefaultPriority = queue.DefaultPriority,
			IssutTypesConfig = new Collection<CreateIssueTypeConfigDto>(
				queue.IssueTypesConfig
					.Select(config => config.ToDto())
					.ToList())
		};
	}

	public static CreateIssueTypeConfigDto ToDto(this IssueTypeConfig config)
	{
		return new CreateIssueTypeConfigDto
		{
			IssueType = config.IssueType.ToString(),
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

	public static Dictionary<string, object> ToDictionary(this IssuesFilter filter)
	{
		var dict = new Dictionary<string, object>();

		foreach (PropertyInfo prop in filter.GetType().GetProperties())
		{
			var value = prop.GetValue(filter, null);
			if (prop.GetValue(filter, null) != null)
			{
				dict.Add(prop.Name, value!);
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
			Clients = project.Clients is not null
					? new Collection<string>(project.Clients.Select(client => client.Id).ToList())
					: null,
			Description = project.Description,
			End = project.End,
			EntityStatus = project.Status,
			Followers = project.Followers is not null
					? new Collection<string>(project.Followers.Select(client => client.Id).ToList())
					: null,
			LeadId = project.Lead?.Id,
			ParentEntityId = project.ParentId,
			Start = project.Start,
			Tags = project.Tags,
			TeamAccess = project.TeamAccess,
			TeamUsers = project.TeamUsers is not null
					? new Collection<string>(project.TeamUsers.Select(client => client.Id).ToList())
					: null,
		};
	}
}
