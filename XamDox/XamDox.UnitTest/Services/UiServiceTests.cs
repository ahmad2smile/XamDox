using NUnit.Framework;
using NUnit.Framework.Internal;
using Xamarin.Forms;
using XamDox.Services;
using XamDox.UnitTest.Mocks;

namespace XamDox.UnitTest.Services
{
	[TestFixture]
	public class UiServiceTests
	{
		public UiServiceTests()
		{
			FormsMock.Init();
		}

		[Test]
		public void UiService_ReturnsPage()
		{
			var uiService = new UiService();

			var result = uiService.GetUi();

			Assert.True(result is Page _);
		}
	}
}
