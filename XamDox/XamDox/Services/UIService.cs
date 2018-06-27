using Xamarin.Forms;

namespace XamDox.Services
{
	public class UiService
	{
		public Page GetUi()
		{
			var appConfig = new AppConfig();
			return appConfig.GetAppInstance();
		}
	}
}
