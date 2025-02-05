using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mindbox.YandexTracker.Tests;

[TestClass]
public class CustomFieldResponseTests
{
	private const string CustomFieldKey = "additionalAssignee";

	private readonly UserShortInfoDto _userInfo = new()
	{
		Id = "3a400bcf-f9ae-4293-8348-e66c2d616437",
		PassportUid = 123511238,
		CloudUid = "51c53703-41eb-4bcd-98e6-cdc5b1eb5484",
		Display = "user1"
	};

	[TestMethod]
	public void GetCustomField_MethodSetUp_MockShouldWork()
	{
		// Arrange
		var mock = new Mock<CustomFieldsResponse>();

		mock.Setup(x => x.GetCustomField<UserShortInfoDto>(It.IsIn(CustomFieldKey)))
			.Returns((string _) => _userInfo);

		// Act
		var result = mock.Object.GetCustomField<UserShortInfoDto>(CustomFieldKey);

		// Assert
		Assert.AreEqual(_userInfo, result);
	}

	[TestMethod]
	public void TryGetCustomField_MethodSetUp_MockShouldWork()
	{
		// Arrange
		var mock = new Mock<CustomFieldsResponse>();

		mock.Setup(x => x.TryGetCustomField(It.IsIn(CustomFieldKey), out It.Ref<UserShortInfoDto>.IsAny!))
			.Returns((string _, out UserShortInfoDto value) =>
			{
				value = _userInfo;
				return true;
			});

		// Act
		var canGetField = mock.Object.TryGetCustomField(CustomFieldKey, out UserShortInfoDto? result);

		// Assert
		Assert.IsTrue(canGetField);
		Assert.AreEqual(_userInfo, result);
	}
}