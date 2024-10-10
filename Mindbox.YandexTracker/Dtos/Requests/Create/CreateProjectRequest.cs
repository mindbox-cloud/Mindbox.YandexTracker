using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
internal sealed record CreateProjectRequest
{
	[DataMember(Name = "fields")]
	public required ProjectFieldsDto Fields { get; init; }
}

[DataContract]
internal sealed record ProjectFieldsDto
{
	[DataMember(EmitDefaultValue = false, Name = "summary")]
	public required string Summary { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "teamAccess")]
	public bool? TeamAccess { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "description")]
	public string? Description { get; init; }

	// У следующих пяти полей тип был изменен на string, но в API трекера требуется int.
	// Причина: Числа, являющиеся идентификаторами в этих полях довольно большие, не влезают даже в long.

	[DataMember(EmitDefaultValue = false, Name = "author")]
	public string? AuthorId { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "lead")]
	public string? LeadId { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "teamUsers")]
	public Collection<string>? TeamUsers { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "clients")]
	public Collection<string>? Clients { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "followers")]
	public Collection<string>? Followers { get; init; }

	// ----------------------------------------------------------

	/// <remarks>
	/// Must have YYYY-MM-DDThh:mm:ss.sss±hhmm format
	/// </remarks>
	[DataMember(EmitDefaultValue = false, Name = "start")]
	public DateTime? Start { get; init; }

	/// <remarks>
	/// Must have YYYY-MM-DDThh:mm:ss.sss±hhmm format
	/// </remarks>
	[DataMember(EmitDefaultValue = false, Name = "end")]
	public DateTime? End { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "tags")]
	public Collection<string>? Tags { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "parentEntity")]
	public int? ParentEntityId { get; init; }

	[DataMember(EmitDefaultValue = false, Name = "entityStatus")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public ProjectEntityStatus? EntityStatus { get; init; }
}