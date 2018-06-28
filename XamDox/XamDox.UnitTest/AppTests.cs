using NUnit.Framework;
using XamDox.UnitTest.Mocks;

namespace XamDox.UnitTest
{
	[TestFixture]
	public class AppTests
	{
		public AppTests()
		{
			FormsMock.Init();
		}

		[Test]
		public void ApplicationIsNotNull()
		{
			var app = new ApplicationMock();

			Assert.NotNull(app);
		}
	}
}
