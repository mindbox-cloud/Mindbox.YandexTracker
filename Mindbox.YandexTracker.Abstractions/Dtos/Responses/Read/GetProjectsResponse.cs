using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record GetProjectsResponse
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
public sealed record ProjectInfo
{
	[DataMember(Name = "id")]
	public int Id { get; set; }

	[DataMember(Name = "entityType")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public ProjectEntityType ProjectType { get; set; }

	[DataMember(Name = "createdBy")]
	public required FieldInfo CreatedBy { get; set; }

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; set; }

	[DataMember(Name = "updatedAt")]
	public required DateTime UpdatedAt { get; set; }

	[DataMember(Name = "fields")]
	public Dictionary<string, object> Fields { get; set; } = [];
}