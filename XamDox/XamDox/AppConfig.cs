using TimeClock;
using Xamarin.Forms;

namespace XamDox
{
	public class AppConfig
	{
		public Page GetAppScreen()
		{
			return new TimeClockApp().AppScreen();
		}
	}
}
