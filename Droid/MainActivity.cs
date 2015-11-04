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
						"", 
						Application.Context)
				),
				typeof(Interfaces.IAnalytics));

			Locator.CurrentMutable.RegisterLazySingleton(
				() => new Xamalytics.Services.Droid.GoogleAnalyticsAnalytics (
					Application.Context, 
					""), 
				typeof(Interfaces.IAnalytics));

			Locator.CurrentMutable.RegisterLazySingleton(
				() => new Xamalytics.Services.Droid.ParseAnalyticsAnalytics(
					"", 
					""), 
				typeof(Interfaces.IAnalytics));

			LoadApplication (new App ());
		}
	}
}

