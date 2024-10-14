using System.Runtime.Serialization;

namespace Mindbox.YandexTracker;

/// <summary>
/// Тип проекта/портфеля
/// </summary>
[DataContract]
public enum ProjectEntityType
{
	/// <summary>
	/// Проект
	/// </summary>
	[EnumMember(Value = "project")]
	Project,

	/// <summary>
	/// Портфель — это набор проектов и других портфелей, объединенных в одно направление
	/// </summary>
	[EnumMember(Value = "portfolio")]
	Portfolio
}
