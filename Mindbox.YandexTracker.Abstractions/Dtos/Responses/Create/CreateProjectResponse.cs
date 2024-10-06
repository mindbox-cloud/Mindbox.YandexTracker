using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record CreateProjectResponse
{
	[DataMember(Name = "id")]
	public required string Id { get; init; }

	[DataMember(Name = "shortId")]
	public required int ShortId { get; init; }

	[DataMember(Name = "entityType")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public ProjectEntityType ProjectEntityType { get; init; }

	[DataMember(Name = "createdBy")]
	public required FieldInfo CreatedBy { get; init; }

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; init; }

	[DataMember(Name = "updatedAt")]
	public DateTime UpdatedAt { get; init; }

	[DataMember(Name = "fields")]
	[JsonExtensionData]
	public Dictionary<string, object> Fields { get; init; } = [];
}
