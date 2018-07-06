using Android.Content;
using Android.Hardware;
using Android.Runtime;
using Android.Views;
using System;
using System.Collections.Generic;
#pragma warning disable 618

namespace XamDox.Droid.Renderers
{
	public sealed class CameraPreview : ViewGroup, ISurfaceHolderCallback
	{
		private readonly SurfaceView _surfaceView;
#pragma warning disable CS0618 // Type or member is obsolete
		private Camera.Size _previewSize;
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning disable CS0618 // Type or member is obsolete
		private IList<Camera.Size> _supportedPreviewSizes;
#pragma warning restore CS0618 // Type or member is obsolete
		private Camera _camera;
		private readonly IWindowManager _windowManager;

		public bool IsPreviewing { get; set; }

		public Camera Preview
		{
			get { return _camera; }
			set
			{
				_camera = value;
				if (_camera != null)
				{
					_supportedPreviewSizes = Preview.GetParameters().SupportedPreviewSizes;
					RequestLayout();
				}
			}
		}

		public CameraPreview(Context context)
			: base(context)
		{
			_surfaceView = new SurfaceView(context);
			AddView(_surfaceView);

			_windowManager = Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

			IsPreviewing = false;
			var holder = _surfaceView.Holder;
			holder.AddCallback(this);
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			var width = ResolveSize(SuggestedMinimumWidth, widthMeasureSpec);
			var height = ResolveSize(SuggestedMinimumHeight, heightMeasureSpec);
			SetMeasuredDimension(width, height);

			if (_supportedPreviewSizes != null)
			{
				_previewSize = GetOptimalPreviewSize(_supportedPreviewSizes, width, height);
			}
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
			var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

			_surfaceView.Measure(msw, msh);
			_surfaceView.Layout(0, 0, r - l, b - t);
		}

		public void SurfaceCreated(ISurfaceHolder holder)
		{
			try
			{
				Preview?.SetPreviewDisplay(holder);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(@"			ERROR: ", ex.Message);
			}
		}

		public void SurfaceDestroyed(ISurfaceHolder holder)
		{
			Preview?.StopPreview();
		}

		public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
		{
			var parameters = Preview.GetParameters();
			parameters.SetPreviewSize(_previewSize.Width, _previewSize.Height);
			RequestLayout();

			switch (_windowManager.DefaultDisplay.Rotation)
			{
				case SurfaceOrientation.Rotation0:
					_camera.SetDisplayOrientation(90);
					break;
				case SurfaceOrientation.Rotation90:
					_camera.SetDisplayOrientation(0);
					break;
				case SurfaceOrientation.Rotation270:
					_camera.SetDisplayOrientation(180);
					break;
			}

			Preview.SetParameters(parameters);
			Preview.StartPreview();
			IsPreviewing = true;
		}

		private static Camera.Size GetOptimalPreviewSize(IList<Camera.Size> sizes, int w, int h)
		{
			const double aspectTolerance = 0.1;
			var targetRatio = (double)w / h;

			if (sizes == null)
			{
				return null;
			}

			Camera.Size optimalSize = null;
			var minDiff = double.MaxValue;

			var targetHeight = h;
			foreach (var size in sizes)
			{
				var ratio = (double)size.Width / size.Height;

				if (Math.Abs(ratio - targetRatio) > aspectTolerance)
					continue;
				if (!(Math.Abs(size.Height - targetHeight) < minDiff))
				{
					continue;
				}

				optimalSize = size;
				minDiff = Math.Abs(size.Height - targetHeight);
			}

			if (optimalSize != null) return optimalSize;
			{
				minDiff = double.MaxValue;
				foreach (var size in sizes)
				{
					if (!(Math.Abs(size.Height - targetHeight) < minDiff))
					{
						continue;
					}

					optimalSize = size;
					minDiff = Math.Abs(size.Height - targetHeight);
				}
			}

			return optimalSize;
		}
	}
}