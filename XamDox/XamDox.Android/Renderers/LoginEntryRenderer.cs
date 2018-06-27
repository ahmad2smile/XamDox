using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamDox.Components.Renderers;
using XamDox.Droid.Renderers;

[assembly: ExportRenderer(typeof(LoginEntry), typeof(LoginEntryRenderer))]
namespace XamDox.Droid.Renderers
{
	public class LoginEntryRenderer : EntryRenderer
	{
		public LoginEntryRenderer(Context context) : base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);
			if (e.NewElement == null) return;

			var view = (LoginEntry)Element;
			if (view.IsCornerRadiusEnable)
			{
				// creating gradient drawable for the curved background  
				var gradientBackground = new GradientDrawable();
				gradientBackground.SetShape(ShapeType.Rectangle);
				gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

				// Thickness of the stroke line  
				gradientBackground.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());

				// Radius for the curves  
				gradientBackground.SetCornerRadius(
					DpToPixels(Context, Convert.ToSingle(view.CornerRadius)));

				// set the background of the   
				Control.SetBackground(gradientBackground);
			}

			// Set padding for the internal text from border  
			Control.SetPadding(
				(int)DpToPixels(Context, Convert.ToSingle(12)), Control.PaddingTop,
				(int)DpToPixels(Context, Convert.ToSingle(12)), Control.PaddingBottom);
		}

		private static float DpToPixels(Context context, float valueInDp)
		{
			var metrics = context.Resources.DisplayMetrics;
			return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
		}
	}
}
