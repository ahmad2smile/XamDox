using NUnit.Framework;
using Xamarin.Forms;
using XamDox.UnitTest.Mocks;

namespace XamDox.UnitTest
{
	[TestFixture]
	public class AppConfigTests
	{
		private readonly AppConfig _appConfigInstance;

		public AppConfigTests()
		{
			FormsMock.Init();

			_appConfigInstance = new AppConfig();

		}

		[Test]
		public void GetAppInstance_ReturnsPage()
		{
			var result = _appConfigInstance.GetAppInstance();

			Assert.True(result is Page _);
		}

	}
}
