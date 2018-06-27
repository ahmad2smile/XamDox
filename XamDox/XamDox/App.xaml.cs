using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamDox.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamDox
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			var uiService = new UiService();

			MainPage = uiService.GetUi();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
