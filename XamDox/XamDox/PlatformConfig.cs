using Xamarin.Forms;
using XamDox.Services;

namespace XamDox
{
	public class PlatformConfig
	{
		public static Page AppUi { get; } = TimeClock.AppConfig.Ui;

		public PlatformConfig()
		{
			//Fire When Photo is Captured
			CapturePhotoService.CapturedImageEvent += (sender, source) =>
			{
				TimeClock.AppConfig.CapturePhotoService.CapturedImageHandler(sender, source);
			};
		}
	}
}