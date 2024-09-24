using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class GetProjectsResponse
{
	[DataMember(Name = "hits")]
	public int Hits { get; set; }

	[DataMember(Name = "pages")]
	public int Pages { get; set; }

	[DataMember(Name = "values")]
	public required Collection<ProjectInfo> Values { get; set; }

	[DataMember(Name = "orderBy")]
	public string? OrderBy { get; set; }
}

[DataContract]
public class ProjectInfo
{
	[DataMember(Name = "shortId")]
	public int ShortId { get; set; }

	[DataMember(Name = "entityType")]
	public EntityType EntityType { get; set; }

	[DataMember(Name = "createdBy")]
	public required FieldInfo CreatedBy { get; set; }

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; set; }

	[DataMember(Name = "updatedAt")]
	public required DateTime UpdatedAt { get; set; }

	[DataMember(Name = "fields")]
	public object? Fields { get; set; }
}