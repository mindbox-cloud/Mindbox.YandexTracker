using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

[DataContract]
public class CreateCommentRequest
{
	[DataMember(Name = "text")]
	public required string Text { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "attachmentIds")]
	public Collection<string>? AttachmentIds { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "summonees")]
	public Collection<string>? Summonees { get; set; }

	[DataMember(EmitDefaultValue = false, Name = "maillistSummonees")]
	public Collection<string>? MaillistSummonees { get; set; }
}

/// <summary>
/// Дополнительные поля, которые будут включены в ответ. Возможен множественный выбор.
/// </summary>

[Flags]
public enum CommentExpandData
{
	Attachments = 1,
	Html = 2,
	All
}