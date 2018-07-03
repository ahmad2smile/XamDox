using Xamarin.Forms;

namespace XamDox
{
	public class AppConfig
	{
		public static Page GetAppScreen()
		{
			return TimeClock.App.AppScreen();
		}
	}
}
