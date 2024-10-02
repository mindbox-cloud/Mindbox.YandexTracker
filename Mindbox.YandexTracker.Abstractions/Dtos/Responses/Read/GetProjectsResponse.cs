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
	public int Hits { get; init; }

	[DataMember(Name = "pages")]
	public int Pages { get; init; }

	[DataMember(Name = "values")]
	public required Collection<ProjectInfo> Values { get; init; }

	[DataMember(Name = "orderBy")]
	public string? OrderBy { get; init; }
}

[DataContract]
public sealed record ProjectInfo
{
	[DataMember(Name = "id")]
	public required string Id { get; init; }

	[DataMember(Name = "entityType")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public ProjectEntityType ProjectType { get; init; }

	[DataMember(Name = "createdBy")]
	public required FieldInfo CreatedBy { get; init; }

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; init; }

	[DataMember(Name = "updatedAt")]
	public required DateTime UpdatedAt { get; init; }

	[DataMember(Name = "fields")]
	public Dictionary<string, object> Fields { get; init; } = [];
}