using Android.Content;
using Android.Hardware;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamDox.Components.Renderers.CameraView;
using XamDox.Droid.Renderers;
using XamDox.Services;

[assembly: Dependency(typeof(CameraViewServiceRenderer))]
[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewServiceRenderer))]
namespace XamDox.Droid.Renderers
{
	public class CameraViewServiceRenderer : ViewRenderer<CameraView, CameraPreview>
	{
		private CameraPreview _cameraPreview;

		protected CameraViewServiceRenderer(Context context) : base(context)
		{
		}


		protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				_cameraPreview = new CameraPreview(Context);
				SetNativeControl(_cameraPreview);
			}

			if (e.OldElement != null)
			{
				// Unsubscribe
				_cameraPreview.Click -= OnCameraPreviewClicked;
			}

			if (e.NewElement == null) return;

#pragma warning disable CS0618 // Type or member is obsolete
			Control.Preview = Camera.Open((int)e.NewElement.Camera);
#pragma warning restore CS0618 // Type or member is obsolete

			// Subscribe
			_cameraPreview.Click += OnCameraPreviewClicked;

			//TODO
			//Implement Photo Capture
			CapturePhotoService.ImageCaptureHandler(ImageSource.FromStream(() => new MemoryStream()));
		}

		private void OnCameraPreviewClicked(object sender, EventArgs e)
		{
			if (_cameraPreview.IsPreviewing)
			{
				_cameraPreview.Preview.StopPreview();
				_cameraPreview.IsPreviewing = false;
			}
			else
			{
				_cameraPreview.Preview.StartPreview();
				_cameraPreview.IsPreviewing = true;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Control.Preview.Release();
			}
			base.Dispose(disposing);
		}
	}
}
