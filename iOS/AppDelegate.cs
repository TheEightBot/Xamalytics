using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Splat;

namespace Xamalytics.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			Locator.CurrentMutable.RegisterLazySingleton (
				() => new Services.XamarinInsightsAnalytics (() =>
					Xamarin.Insights.Initialize(
						"0ebf745cf404d091722aeddadab8d87036c74093")
				),
				typeof(Interfaces.IAnalytics));

			Locator.CurrentMutable.RegisterLazySingleton(
				() => new Xamalytics.Services.iOS.GoogleAnalyticsAnalytics(
					"UA-69546843-1"), 
				typeof(Interfaces.IAnalytics));

			Locator.CurrentMutable.RegisterLazySingleton(
				() => new Xamalytics.Services.iOS.ParseAnalyticsAnalytics(
					"9ZKqtWPpTrhU7rUmhmacx7g5TeK3KSo9ppid1bGt", 
					"mTNndKp45OPNhPMG3J9LdDzqA6Xk8KtMI3oDBIGU"), 
				typeof(Interfaces.IAnalytics));

			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
			#endif

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

