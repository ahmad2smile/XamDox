using CoreGraphics;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamDox.Components.Renderers;
using XamDox.iOS.Renderers;

[assembly: ExportRenderer(typeof(LoginEntry), typeof(LoginEntryRenderer))]
namespace XamDox.iOS.Renderers
{
	public class LoginEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null) return;

			var loginEntry = (LoginEntry)Element;

			Control.LeftView = new UIView(new CGRect(0f, 0f, 9f, 20f));
			Control.LeftViewMode = UITextFieldViewMode.Always;

			Control.KeyboardAppearance = UIKeyboardAppearance.Dark;
			Control.ReturnKeyType = UIReturnKeyType.Done;
			// Radius for the curves  
			Control.Layer.CornerRadius = Convert.ToSingle(loginEntry.CornerRadius);
			// Thickness of the Border Color  
			Control.Layer.BorderColor = loginEntry.BorderColor.ToCGColor();
			// Thickness of the Border Width  
			Control.Layer.BorderWidth = loginEntry.BorderWidth;
			Control.ClipsToBounds = true;
		}
	}
}