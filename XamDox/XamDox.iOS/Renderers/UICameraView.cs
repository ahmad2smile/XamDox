using AVFoundation;
using CoreGraphics;
using Foundation;
using System;
using System.Linq;
using UIKit;
using XamDox.Components.Renderers.CameraView;

namespace XamDox.iOS.Renderers
{
	public sealed class UiCameraView : UIView
	{
		private AVCaptureVideoPreviewLayer _previewLayer;
		private readonly CameraOptions _cameraOptions;

		public event EventHandler<EventArgs> Tapped;

		public AVCaptureSession CaptureSession { get; private set; }

		public bool IsPreviewing { get; set; }

		public UiCameraView(CameraOptions options)
		{
			_cameraOptions = options;
			IsPreviewing = false;
			Initialize();
		}

		public override void Draw(CGRect rect)
		{
			base.Draw(rect);
			_previewLayer.Frame = rect;
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);
			OnTapped();
		}

		private void OnTapped()
		{
			var eventHandler = Tapped;
			eventHandler?.Invoke(this, new EventArgs());
		}

		private void Initialize()
		{
			CaptureSession = new AVCaptureSession();
			_previewLayer = new AVCaptureVideoPreviewLayer(CaptureSession)
			{
				Frame = Bounds,
				VideoGravity = AVLayerVideoGravity.ResizeAspectFill
			};

			var videoDevices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
			var cameraPosition = (_cameraOptions == CameraOptions.Front) ? AVCaptureDevicePosition.Front : AVCaptureDevicePosition.Back;
			var device = videoDevices.FirstOrDefault(d => d.Position == cameraPosition);

			if (device == null)
			{
				return;
			}

			var input = new AVCaptureDeviceInput(device, out _);
			CaptureSession.AddInput(input);
			Layer.AddSublayer(_previewLayer);
			CaptureSession.StartRunning();
			IsPreviewing = true;
		}
	}
}