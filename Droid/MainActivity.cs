using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Splat;

namespace Xamalytics.Droid
{
	[Activity (Label = "Xamalytics.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			Locator.CurrentMutable.Register (
				() => new Services.XamarinInsightsAnalytics (() =>
					Xamarin.Insights.Initialize(
						"0ebf745cf404d091722aeddadab8d87036c74093", 
						Application.Context)
				),
				typeof(Interfaces.IAnalytics));

			Locator.CurrentMutable.RegisterLazySingleton(
				() => new Xamalytics.Services.Droid.GoogleAnalyticsAnalytics (
					Application.Context, 
					"UA-69546843-1"), 
				typeof(Interfaces.IAnalytics));

			Locator.CurrentMutable.RegisterLazySingleton(
				() => new Xamalytics.Services.Droid.ParseAnalyticsAnalytics(
					"9ZKqtWPpTrhU7rUmhmacx7g5TeK3KSo9ppid1bGt", 
					"mTNndKp45OPNhPMG3J9LdDzqA6Xk8KtMI3oDBIGU"), 
				typeof(Interfaces.IAnalytics));

			LoadApplication (new App ());
		}
	}
}

