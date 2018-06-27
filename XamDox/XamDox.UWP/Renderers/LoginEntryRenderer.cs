using Windows.UI.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using XamDox.Components.Renderers;
using XamDox.UWP.Renderers;

[assembly: ExportRenderer(typeof(LoginEntry), typeof(LoginEntryRenderer))]
namespace XamDox.UWP.Renderers
{
	public class LoginEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null || Control == null) return;

			Control.BorderThickness = new Windows.UI.Xaml.Thickness(0);
			Control.BorderBrush = Background;
			Control.VerticalContentAlignment = VerticalAlignment.Bottom;
			Control.Padding = new Windows.UI.Xaml.Thickness(0, 10, 0, 0);
		}
	}
}

