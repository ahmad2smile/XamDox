using Xamarin.Forms;

namespace XamDox.Components.Renderers
{
	public class GradientLayout : StackLayout
	{
		public string ColorsList { get; set; }

		public Color[] Colors
		{
			get
			{
				var hex = ColorsList.Split(',');
				var colors = new Color[hex.Length];

				for (var i = 0; i < hex.Length; i++)
				{
					colors[i] = Color.FromHex(hex[i].Trim());
				}

				return colors;
			}
		}

		public GradientColorStackMode Mode { get; set; }
	}

	public enum GradientColorStackMode
	{
		ToRight,
		ToLeft,
		ToTop,
		ToBottom,
		ToTopLeft,
		ToTopRight,
		ToBottomLeft,
		ToBottomRight
	}

}
