using NUnit.Framework;
using Xamarin.Forms;
using XamDox.UnitTest.Mocks;

namespace XamDox.UnitTest
{
	[TestFixture]
	public class AppConfigTests
	{
		public AppConfigTests()
		{
			FormsMock.Init();
		}

		[Test]
		public void GetAppInstance_ReturnsPage()
		{
			var result = AppConfig.AppScreen;

			Assert.True(result is Page _);
		}

	}
}
