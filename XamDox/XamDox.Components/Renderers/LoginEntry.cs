using Xamarin.Forms;

namespace XamDox.Components.Renderers
{
	public class LoginEntry : Entry
	{
		public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(LoginEntry), Color.White);
		public Color BorderColor
		{
			get => (Color)GetValue(BorderColorProperty);
			set => SetValue(BorderColorProperty, value);
		}

		public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(LoginEntry), 10);
		public int BorderWidth
		{
			get => (int)GetValue(BorderWidthProperty);
			set => SetValue(BorderWidthProperty, value);
		}

		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(LoginEntry), 10.0);
		public double CornerRadius
		{
			get => (double)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}

		public static readonly BindableProperty IsCornerRadiusEnleProperty = BindableProperty.Create(nameof(IsCornerRadiusEnable), typeof(bool), typeof(LoginEntry), true);
		public bool IsCornerRadiusEnable
		{
			get => (bool)GetValue(IsCornerRadiusEnleProperty);
			set => SetValue(IsCornerRadiusEnleProperty, value);
		}

	}
}
