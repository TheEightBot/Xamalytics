using System;

using Xamarin.Forms;
using Splat;
using System.Threading.Tasks;

namespace Xamalytics
{
	public class App : Application
	{
		public App ()
		{
			Locator.CurrentMutable.RegisterLazySingleton (
				() => new Services.DebugDiagnosticsAnalytics (),
				typeof(Interfaces.IAnalytics));

			Locator.CurrentMutable.RegisterLazySingleton (
				() => new Services.AmazonMobileAnalytics (
					"",
					""
				),
				typeof(Interfaces.IAnalytics));

			Locator.CurrentMutable.RegisterLazySingleton (
				() => new Services.SegmentAnalyticsAnalytics (
					""
				),
				typeof(Interfaces.IAnalytics));

			var analyticsServices = Locator.CurrentMutable.GetServices<Interfaces.IAnalytics> ();

			foreach (var analyticsService in analyticsServices)
				analyticsService.Init ();

			// The root page of your application
			MainPage = new NavigationPage(new Pages.Login()) { 
				BarBackgroundColor = Color.FromRgb(1, 61, 100),
				BarTextColor = Color.White
			};
		}

		protected override void OnStart ()
		{
			
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{			
			// Handle when your app sleeps
			var analyticsServices = Locator.CurrentMutable.GetServices<Interfaces.IAnalytics> ();

			foreach (var analyticsService in analyticsServices)
				analyticsService.Enabled = false;

		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
			var analyticsServices = Locator.CurrentMutable.GetServices<Interfaces.IAnalytics> ();

			foreach (var analyticsService in analyticsServices)
				analyticsService.Enabled = true;
		}
	}
}

