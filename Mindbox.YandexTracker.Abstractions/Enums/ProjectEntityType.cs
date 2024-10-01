using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Тип проекта/портфеля
/// </summary>
[DataContract]
public enum ProjectEntityType
{
	[EnumMember(Value = "project")]
	Project,

	[EnumMember(Value = "portfolio")]
	Portfolio
}
