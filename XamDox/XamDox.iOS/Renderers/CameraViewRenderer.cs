using AVFoundation;
using Foundation;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamDox.Components.Renderers.CameraView;
using XamDox.iOS.Renderers;
using XamDox.Services;

[assembly: Dependency(typeof(CameraViewRenderer))]
[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]
namespace XamDox.iOS.Renderers
{
	public class CameraViewRenderer : ViewRenderer<CameraView, UiCameraView>
	{
		private UiCameraView _uiCameraView;
		private AVCaptureStillImageOutput _avCaptureStillImage;

		protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			base.OnElementChanged(e);


			if (Control == null)
			{
				_uiCameraView = new UiCameraView(e.NewElement.Camera);
				SetNativeControl(_uiCameraView);
			}
			if (e.OldElement != null)
			{
				_uiCameraView.Tapped -= OnCameraViewTapped;
			}
			if (e.NewElement != null)
			{
				_uiCameraView.Tapped += OnCameraViewTapped;
			}

			_avCaptureStillImage = new AVCaptureStillImageOutput
			{
				OutputSettings = new NSDictionary()
			};

			Control.CaptureSession.AddOutput(_avCaptureStillImage);
		}

		private async void OnCameraViewTapped(object sender, EventArgs e)
		{
			if (_uiCameraView.IsPreviewing)
			{
				var imageData = await CapturePhoto();


				CapturePhotoService.ImageCaptureHandler(ImageSource.FromStream(() => new MemoryStream(imageData)));

				SetNeedsDisplay();

				_uiCameraView.CaptureSession.StopRunning();
				_uiCameraView.IsPreviewing = false;
			}
			else
			{
				_uiCameraView.CaptureSession.StartRunning();
				_uiCameraView.IsPreviewing = true;
			}
		}

		private async Task<byte[]> CapturePhoto()
		{
			var videoConnection = _avCaptureStillImage.ConnectionFromMediaType(AVMediaType.Video);
			var sampleBuffer = await _avCaptureStillImage.CaptureStillImageTaskAsync(videoConnection);

			var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);

			var imageData = jpegImageAsNsData.ToArray();

			return imageData;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Control.CaptureSession.Dispose();
				Control.Dispose();
			}
			base.Dispose(disposing);
		}

	}
}
