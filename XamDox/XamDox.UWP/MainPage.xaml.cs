namespace XamDox.UWP
{
	public sealed partial class MainPage
	{
		public MainPage()
		{
			this.InitializeComponent();

			LoadApplication(new XamDox.App());
		}
	}
}
