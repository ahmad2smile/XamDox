using CoreAnimation;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamDox.Components.Renderers;
using XamDox.iOS.Renderers;

[assembly: ExportRenderer(typeof(GradientLayout), typeof(GradientLayoutRenderer))]
namespace XamDox.iOS.Renderers
{
	public class GradientLayoutRenderer : VisualElementRenderer<StackLayout>
	{
		public override void Draw(CGRect rect)
		{
			base.Draw(rect);
			var layout = (GradientLayout)Element;

			var colors = new CGColor[layout.Colors.Length];

			for (int i = 0, l = colors.Length; i < l; i++)
			{
				colors[i] = layout.Colors[i].ToCGColor();
			}

			var gradientLayer = new CAGradientLayer();

			switch (layout.Mode)
			{
				case GradientColorStackMode.ToRight:
					break;
				default:
					gradientLayer.StartPoint = new CGPoint(0, 0.5);
					gradientLayer.EndPoint = new CGPoint(1, 0.5);
					break;
				case GradientColorStackMode.ToLeft:
					gradientLayer.StartPoint = new CGPoint(1, 0.5);
					gradientLayer.EndPoint = new CGPoint(0, 0.5);
					break;
				case GradientColorStackMode.ToTop:
					gradientLayer.StartPoint = new CGPoint(0.5, 0);
					gradientLayer.EndPoint = new CGPoint(0.5, 1);
					break;
				case GradientColorStackMode.ToBottom:
					gradientLayer.StartPoint = new CGPoint(0.5, 1);
					gradientLayer.EndPoint = new CGPoint(0.5, 0);
					break;
				case GradientColorStackMode.ToTopLeft:
					gradientLayer.StartPoint = new CGPoint(1, 0);
					gradientLayer.EndPoint = new CGPoint(0, 1);
					break;
				case GradientColorStackMode.ToTopRight:
					gradientLayer.StartPoint = new CGPoint(0, 1);
					gradientLayer.EndPoint = new CGPoint(1, 0);
					break;
				case GradientColorStackMode.ToBottomLeft:
					gradientLayer.StartPoint = new CGPoint(1, 1);
					gradientLayer.EndPoint = new CGPoint(0, 0);
					break;
				case GradientColorStackMode.ToBottomRight:
					gradientLayer.StartPoint = new CGPoint(0, 0);
					gradientLayer.EndPoint = new CGPoint(1, 1);
					break;
			}

			gradientLayer.Frame = rect;
			gradientLayer.Colors = colors;

			NativeView.Layer.InsertSublayer(gradientLayer, 0);
		}
	}
}
