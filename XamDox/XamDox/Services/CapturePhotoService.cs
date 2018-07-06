using System;
using Xamarin.Forms;

namespace XamDox.Services
{
	public class CapturePhotoService
	{
		public static event EventHandler<ImageSource> CapturedImageEvent;

		public static void ImageCaptureHandler(ImageSource e)
		{
			CapturedImageEvent?.Invoke(new CapturePhotoService(), e);
		}
	}
}