using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Тип файла
/// </summary>
[DataContract]
public enum FileType
{
	/// <summary>
	/// Файл
	/// </summary>
	[EnumMember(Value = "text/plain")]
	TxtFile,

	/// <summary>
	/// Изображение
	/// </summary>
	[EnumMember(Value = "image/png")]
	ImageFile
}
