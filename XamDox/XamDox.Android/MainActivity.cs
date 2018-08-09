using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

namespace XamDox.Droid
{
	[Activity(Label = "XamDox", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{
		public const int CameraPermissionsCode = 1;

		public static readonly string[] CameraPermissions =
		{
			Manifest.Permission.Camera
		};

		public static event EventHandler CameraPermissionGranted;
		private View _layout;


		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			Forms.Init(this, bundle);
			LoadApplication(new App());

			_layout = FindViewById(Resource.Id.action_bar_root);

		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			if (requestCode == CameraPermissionsCode && grantResults[0] == Permission.Denied)
			{
				Snackbar.Make(_layout, "Camera permission is denied. Please allow Camera use.", Snackbar.LengthIndefinite)
					.SetAction("OK", v => RequestPermissions(CameraPermissions, CameraPermissionsCode))
					.Show();
				return;
			}

			CameraPermissionGranted?.Invoke(this, EventArgs.Empty);
		}
	}
}

