using Android.Content;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamDox.Components.Renderers;
using XamDox.Droid.Renderers;

[assembly: ExportRenderer(typeof(GradientLayout), typeof(GradientLayoutRenderer))]
namespace XamDox.Droid.Renderers
{
	public class GradientLayoutRenderer : VisualElementRenderer<StackLayout>
	{
		public Color[] Colors { get; set; }

		public GradientColorStackMode Mode { get; set; }

		public GradientLayoutRenderer(Context ctx) : base(ctx)
		{ }

		protected override void DispatchDraw(Android.Graphics.Canvas canvas)
		{
			if (Colors == null)
			{
				base.DispatchDraw(canvas);
				return;
			}

			Android.Graphics.LinearGradient gradient;

			var colors = new int[Colors.Length];

			for (int i = 0, l = Colors.Length; i < l; i++)
			{
				colors[i] = Colors[i].ToAndroid().ToArgb();
			}

			switch (Mode)
			{
				default:
					gradient = new Android.Graphics.LinearGradient(0, 0, Width, 0, colors, null, Android.Graphics.Shader.TileMode.Mirror);
					break;
				case GradientColorStackMode.ToLeft:
					gradient = new Android.Graphics.LinearGradient(Width, 0, 0, 0, colors, null, Android.Graphics.Shader.TileMode.Mirror);
					break;
				case GradientColorStackMode.ToTop:
					gradient = new Android.Graphics.LinearGradient(0, Height, 0, 0, colors, null, Android.Graphics.Shader.TileMode.Mirror);
					break;
				case GradientColorStackMode.ToBottom:
					gradient = new Android.Graphics.LinearGradient(0, 0, 0, Height, colors, null, Android.Graphics.Shader.TileMode.Mirror);
					break;
				case GradientColorStackMode.ToTopLeft:
					gradient = new Android.Graphics.LinearGradient(Width, Height, 0, 0, colors, null, Android.Graphics.Shader.TileMode.Mirror);
					break;
				case GradientColorStackMode.ToTopRight:
					gradient = new Android.Graphics.LinearGradient(0, Height, Width, 0, colors, null, Android.Graphics.Shader.TileMode.Mirror);
					break;
				case GradientColorStackMode.ToBottomLeft:
					gradient = new Android.Graphics.LinearGradient(Width, 0, 0, Height, colors, null, Android.Graphics.Shader.TileMode.Mirror);
					break;
				case GradientColorStackMode.ToBottomRight:
					gradient = new Android.Graphics.LinearGradient(0, 0, Width, Height, colors, null, Android.Graphics.Shader.TileMode.Mirror);
					break;
			}

			var paint = new Android.Graphics.Paint
			{
				Dither = true,
			};

			paint.SetShader(gradient);
			canvas.DrawPaint(paint);

			base.DispatchDraw(canvas);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
				return;

			try
			{
				var layout = (GradientLayout)e.NewElement;

				Colors = layout.Colors;
				Mode = layout.Mode;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
			}
		}
	}
}
