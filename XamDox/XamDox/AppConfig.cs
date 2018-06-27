using TimeClock;
using Xamarin.Forms;

namespace XamDox
{
	public class AppConfig
	{
		public Page GetAppInstance()
		{
			return new MainScreen();
		}
	}
}
