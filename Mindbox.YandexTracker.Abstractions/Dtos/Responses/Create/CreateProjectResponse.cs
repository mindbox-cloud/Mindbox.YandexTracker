using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record CreateProjectResponse
{
	[DataMember(Name = "id")]
	public required string Id { get; set; }

	[DataMember(Name = "entityType")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public ProjectEntityType ProjectEntityType { get; set; }

	[DataMember(Name = "createdBy")]
	public required FieldInfo CreatedBy { get; set; }

	[DataMember(Name = "createdAt")]
	public DateTime CreatedAt { get; set; }

	[DataMember(Name = "updatedAt")]
	public DateTime UpdatedAt { get; set; }

	[DataMember(Name = "fields")]
	[JsonExtensionData]
	public Dictionary<string, object> Fields { get; set; } = [];
}
