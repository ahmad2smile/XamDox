using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace XamDox
{
	public partial class App
	{
		public App()
		{
			InitializeComponent();

			var _ = new PlatformConfig();

			MainPage = PlatformConfig.AppUi;
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