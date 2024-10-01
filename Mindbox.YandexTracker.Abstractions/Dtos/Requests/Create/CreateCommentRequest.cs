using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public sealed record CreateCommentRequest
{
	[DataMember(Name = "text")]
	public required string Text { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "attachmentIds")]
	public Collection<string>? AttachmentIds { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "summonees")]
	public Collection<string>? Summonees { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "maillistSummonees")]
	public Collection<string>? MaillistSummonees { get; set; }

	[JsonIgnore]
	public bool? IsAddToFollowers { get; set; }
}