using FaceDetection_UWP;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Enumeration;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.System.Display;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using XamDox.Components.Renderers.CameraView;
using XamDox.Services;
using XamDox.UWP.Renderers;
using Application = Windows.UI.Xaml.Application;
using FlowDirection = Windows.UI.Xaml.FlowDirection;

[assembly: Dependency(typeof(CameraViewServiceRenderer))]
[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewServiceRenderer))]
namespace XamDox.UWP.Renderers
{
	public class CameraViewServiceRenderer : ViewRenderer<CameraView, CaptureElement>
	{
		//		[DllImport("FaceDetection_UWP.dll")]
		//		private static extern int setupFaceDetection(Xamarin.Forms.ImageSource imgSrc);

		private readonly DisplayInformation _displayInformation = DisplayInformation.GetForCurrentView();
		private DisplayOrientations _displayOrientation = DisplayOrientations.Portrait;
		private readonly DisplayRequest _displayRequest = new DisplayRequest();

		// Rotation metadata to apply to preview stream (https://msdn.microsoft.com/en-us/library/windows/apps/xaml/hh868174.aspx)
		private static readonly Guid RotationKey = new Guid("C380465D-2271-428C-9B83-ECEA3B4A85C1"); // (MF_MT_VIDEO_ROTATION)

		private static readonly SemaphoreSlim MediaCaptureLifeLock = new SemaphoreSlim(1);

		private MediaCapture _mediaCapture;
		private CaptureElement _captureElement;
		private bool _isInitialized;
		private bool _isPreviewing;
		private bool _externalCamera;
		private bool _mirroringPreview;

		private Application _app;

		protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				_app = Application.Current;
				_app.Suspending += OnAppSuspending;
				_app.Resuming += OnAppResuming;

				_captureElement = new CaptureElement { Stretch = Stretch.UniformToFill };

				SetupCamera();
				SetNativeControl(_captureElement);
			}
			if (e.OldElement != null)
			{
				Tapped -= OnCameraPreviewTapped;
				_displayInformation.OrientationChanged -= OnOrientationChanged;
				_app.Suspending -= OnAppSuspending;
				_app.Resuming -= OnAppResuming;
			}
			if (e.NewElement != null)
			{
				Tapped += OnCameraPreviewTapped;
			}
		}

		private async void SetupCamera()
		{
			_displayOrientation = _displayInformation.CurrentOrientation;
			_displayInformation.OrientationChanged += OnOrientationChanged;
			await InitializeCameraAsync();
		}

		private async void OnOrientationChanged(DisplayInformation sender, object args)
		{
			_displayOrientation = sender.CurrentOrientation;
			if (_isPreviewing)
			{
				await SetPreviewRotationAsync();
			}
		}

		private async void OnCameraPreviewTapped(object sender, TappedRoutedEventArgs e)
		{
			if (_isPreviewing)
			{
				await StopPreviewAsync();
			}
			else
			{
				await StartPreviewAsync();
			}
		}

		private async Task InitializeCameraAsync()
		{
			await MediaCaptureLifeLock.WaitAsync();

			if (_mediaCapture == null)
			{
				// Attempt to get the back camera, but use any camera if not
				var cameraDevice = await FindCameraDeviceByPanelAsync(Windows.Devices.Enumeration.Panel.Front);
				if (cameraDevice == null)
				{
					Debug.WriteLine("No camera found");
					return;
				}

				_mediaCapture = new MediaCapture();
				var settings = new MediaCaptureInitializationSettings { VideoDeviceId = cameraDevice.Id };
				try
				{
					await _mediaCapture.InitializeAsync(settings);
					_isInitialized = true;
				}
				catch (UnauthorizedAccessException)
				{
					Debug.WriteLine("Camera access denied");
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Exception initializing MediaCapture - {0}: {1}", cameraDevice.Id, ex);
				}
				finally
				{
					MediaCaptureLifeLock.Release();
				}

				if (_isInitialized)
				{
					if (cameraDevice.EnclosureLocation == null || cameraDevice.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Unknown)
					{
						_externalCamera = true;
					}
					else
					{
						// Camera is on device
						_externalCamera = false;

						// Mirror preview if camera is on front panel
						_mirroringPreview = (cameraDevice.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
					}
					await StartPreviewAsync();
				}
			}
			else
			{
				MediaCaptureLifeLock.Release();
			}
		}

		private async Task StartPreviewAsync()
		{
			// Prevent the device from sleeping while the preview is running
			_displayRequest.RequestActive();

			// Setup preview source in UI and mirror if required
			_captureElement.Source = _mediaCapture;
			_captureElement.FlowDirection = _mirroringPreview ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

			// Start preview
			await _mediaCapture.StartPreviewAsync();
			_isPreviewing = true;

			if (_isPreviewing)
			{
				await SetPreviewRotationAsync();
			}
		}

		private async Task StopPreviewAsync()
		{
			await CapturePhoto();

			_isPreviewing = false;
			await _mediaCapture.StopPreviewAsync();

			await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				// Allow device screen to sleep now preview is stopped
				_displayRequest.RequestRelease();
			});
		}

		private async Task SetPreviewRotationAsync()
		{
			// Only update the orientation if the camera is mounted on the device
			if (_externalCamera)
			{
				return;
			}

			// Derive the preview rotation
			int rotation = ConvertDisplayOrientationToDegrees(_displayOrientation);

			// Invert if mirroring
			if (_mirroringPreview)
			{
				rotation = (360 - rotation) % 360;
			}

			// Add rotation metadata to preview stream
			var props = _mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview);
			props.Properties.Add(RotationKey, rotation);
			await _mediaCapture.SetEncodingPropertiesAsync(MediaStreamType.VideoPreview, props, null);
		}

		private async Task CleanupCameraAsync()
		{
			await MediaCaptureLifeLock.WaitAsync();

			if (_isInitialized)
			{
				if (_isPreviewing)
				{
					await StopPreviewAsync();
				}
				_isInitialized = false;
			}
			if (_captureElement != null)
			{
				_captureElement.Source = null;
			}
			if (_mediaCapture != null)
			{
				_mediaCapture.Dispose();
				_mediaCapture = null;
			}
		}

		private static async Task<DeviceInformation> FindCameraDeviceByPanelAsync(Windows.Devices.Enumeration.Panel desiredPanel)
		{

			var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
			var desiredDevice = allVideoDevices.FirstOrDefault(d => d.EnclosureLocation != null && d.EnclosureLocation.Panel == desiredPanel);
			return desiredDevice ?? allVideoDevices.FirstOrDefault();
		}

		private static int ConvertDisplayOrientationToDegrees(DisplayOrientations orientation)
		{
			switch (orientation)
			{
				case DisplayOrientations.Portrait:
					return 90;
				case DisplayOrientations.LandscapeFlipped:
					return 180;
				case DisplayOrientations.PortraitFlipped:
					return 270;
				default:
					return 0;
			}
		}

		private async void OnAppSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			await CleanupCameraAsync();
			_displayInformation.OrientationChanged -= OnOrientationChanged;
			deferral.Complete();
		}

		private void OnAppResuming(object sender, object o)
		{
			_displayOrientation = _displayInformation.CurrentOrientation;
			_displayInformation.OrientationChanged += OnOrientationChanged;
		}

		private async Task CapturePhoto()
		{
			var lowLagCapture = await _mediaCapture.PrepareLowLagPhotoCaptureAsync(ImageEncodingProperties.CreateJpeg());

			var capturedPhoto = await lowLagCapture.CaptureAsync();
			var softwareBitmap = capturedPhoto.Frame.AsStream();

			var imageSource = Xamarin.Forms.ImageSource.FromStream(() => softwareBitmap);

			CapturePhotoService.ImageCaptureHandler(imageSource);

			await lowLagCapture.FinishAsync();

			try
			{
				var gg = new FaceDetectionBridge();
				var g = gg.setupFaceDetection(new byte());
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}
