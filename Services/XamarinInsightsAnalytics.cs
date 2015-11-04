using System;
using System.Collections.Generic;

namespace Xamalytics.Services
{
	public class XamarinInsightsAnalytics : Interfaces.IAnalytics
	{
		#region IAnalyticsService implementation

		public bool Enabled {
			get;
			set;
		}

		Action _insightsInitializer;

		public XamarinInsightsAnalytics(Action insightsInitializer) {
			if (insightsInitializer == null)
				throw new ArgumentNullException ("insightsInitializer");

			_insightsInitializer = insightsInitializer;
		}

		public void Init ()
		{
			_insightsInitializer.Invoke ();

			Enabled = Xamarin.Insights.IsInitialized;
		}

		public void SetUser (string identifier, Dictionary<string, string> userProperties)
		{
			if (Enabled && Xamarin.Insights.IsInitialized)
				Xamarin.Insights.Identify (identifier, userProperties);
		}

		public void LogViewLoaded (string name)
		{
			if (Enabled && Xamarin.Insights.IsInitialized)
				Xamarin.Insights.Track (
					Constants.Tracking.ViewLoaded,
					new Dictionary<string, string>(){
						{ Constants.Tracking.ViewName, name }
					}
				);
		}

		public void LogViewUnloaded (string name)
		{
			if (Enabled && Xamarin.Insights.IsInitialized)
				Xamarin.Insights.Track (
					Constants.Tracking.ViewUnloaded,
					new Dictionary<string, string>(){
						{ Constants.Tracking.ViewName, name }
					}
				);
		}

		public void LogViewDisplayed (string name)
		{
			if (Enabled && Xamarin.Insights.IsInitialized)
				Xamarin.Insights.Track (
					Constants.Tracking.ViewDisplayed,
					new Dictionary<string, string>(){
						{ Constants.Tracking.ViewName, name }
					}
				);
		}

		public void LogViewHidden (string name)
		{
			if (Enabled && Xamarin.Insights.IsInitialized)
				Xamarin.Insights.Track (
					Constants.Tracking.ViewHidden,
					new Dictionary<string, string>(){
						{ Constants.Tracking.ViewName, name }
					}
				);
		}

		public void LogError (string name, string title, string message){
			if (Enabled && Xamarin.Insights.IsInitialized)
				Xamarin.Insights.Track (
					Constants.Tracking.UserError,
					new Dictionary<string, string>(){
						{ Constants.Tracking.UserErrorView, name },
						{ Constants.Tracking.UserErrorTitle, title },
						{ Constants.Tracking.UserErrorMessage, message }
					}
				);
		}

		public void LogEvent (string name, string category, string eventName, string eventData){
			if (Enabled && Xamarin.Insights.IsInitialized)
				Xamarin.Insights.Track (
					name,
					new Dictionary<string, string>(){
						{ Constants.Tracking.Category, category },
						{ Constants.Tracking.EventName, eventName },
						{ Constants.Tracking.EventData, eventData },
					}
				);
		}

		public void LogPerformance (string category, TimeSpan performanceCounter, Dictionary<string, string> values = null)
		{
			if (values == null)
				values = new Dictionary<string, string> ();

			values ["Duration"] = ((long)performanceCounter.TotalMilliseconds).ToString ();

			if (Enabled && Xamarin.Insights.IsInitialized)
				Xamarin.Insights.Track (
					String.Format("Performance - {0}", category),
					values
				);
		}

		#endregion
	}
}

