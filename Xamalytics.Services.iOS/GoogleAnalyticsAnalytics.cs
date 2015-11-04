using System;
using System.Collections.Generic;
using Google.Analytics;

namespace Xamalytics.Services.iOS
{
	public class GoogleAnalyticsAnalytics : Interfaces.IAnalytics
	{
		public GoogleAnalyticsAnalytics ()
		{
		}

		private ITracker _tracker;
		private string _trackingId;

		public GoogleAnalyticsAnalytics(string trackingId){
			_trackingId = trackingId;
		}

		#region IAnalytics implementation
		public bool Enabled {
			get;
			set;
		}

		public void Init ()
		{
			// Optional: set Google Analytics dispatch interval to e.g. 20 seconds.
			Gai.SharedInstance.DispatchInterval = 20;

			// Optional: automatically send uncaught exceptions to Google Analytics.
			Gai.SharedInstance.TrackUncaughtExceptions = true;

			if (System.Diagnostics.Debugger.IsAttached)
				Gai.SharedInstance.Logger.SetLogLevel (LogLevel.Verbose);

			_tracker = Gai.SharedInstance.GetTracker (_trackingId);

			Enabled = _tracker != null;
		}

		/// <remarks>Google is super not cool with tracking people with our own custom information...</remarks>
		public void SetUser (string identifier, Dictionary<string, string> userProperties)
		{
		}

		public void LogViewLoaded (string name)
		{
			if (Enabled && _tracker != null) {
				_tracker.Set (GaiConstants.ScreenName, name);

				_tracker.Send (DictionaryBuilder.CreateScreenView ().Build ());

				_tracker.Send(
					DictionaryBuilder.CreateEvent(
						Constants.Tracking.UserInterfaceEvent,
						Constants.Tracking.ViewDisplayed,
						name,
						null
					)
					.Build()
				);

				_tracker.Set (GaiConstants.ScreenName,null);
			}
		}

		public void LogViewUnloaded (string name)
		{
			if (Enabled && _tracker != null) {
				_tracker.Set (GaiConstants.ScreenName, name);

				_tracker.Send(
					DictionaryBuilder.CreateEvent(
						Constants.Tracking.UserInterfaceEvent,
						Constants.Tracking.ViewUnloaded,
						name,
						null
					)
					.Build()
				);

				_tracker.Set (GaiConstants.ScreenName,null);
			}
		}

		public void LogViewDisplayed (string name)
		{
			if (Enabled && _tracker != null) {
				_tracker.Set (GaiConstants.ScreenName, name);

				_tracker.Send(
					DictionaryBuilder.CreateEvent(
						Constants.Tracking.UserInterfaceEvent,
						Constants.Tracking.ViewDisplayed,
						name,
						null
					)
					.Build()
				);

				_tracker.Set (GaiConstants.ScreenName,null);
			}
		}

		public void LogViewHidden (string name)
		{
			if (Enabled && _tracker != null) {
				_tracker.Set (GaiConstants.ScreenName, name);

				_tracker.Send(
					DictionaryBuilder.CreateEvent(
						Constants.Tracking.UserInterfaceEvent,
						Constants.Tracking.ViewHidden,
						name,
						null
					)
					.Build()
				);

				_tracker.Set (GaiConstants.ScreenName,null);
			}
		}

		public void LogError (string name, string title, string message)
		{
			if (Enabled && _tracker != null) {
				_tracker.Set (GaiConstants.ScreenName, name);

				_tracker.Send(
					DictionaryBuilder.CreateException(
						string.Format("name: {0}\ttitle: {1}\tmessage: {2}", name, title, message),
						false
					)
					.Build()
				);

				_tracker.Set (GaiConstants.ScreenName,null);
			}
		}

		public void LogEvent (string name, string category, string eventName, string eventData){
			if (Enabled && _tracker != null) {
				_tracker.Set (GaiConstants.ScreenName, name);

				_tracker.Send(
					DictionaryBuilder.CreateEvent(
						category,
						eventName,
						eventData,
						null
					)
					.Build()
				);

				_tracker.Set (GaiConstants.ScreenName,null);
			}
		}

		public void LogPerformance (string category, TimeSpan performanceCounter, Dictionary<string, string> values = null)
		{
			if (Enabled && _tracker != null) {
				_tracker.Send(
					DictionaryBuilder.CreateTiming(
						category,
						(long)performanceCounter.TotalMilliseconds,
						null,
						null
					)
					.Build()
				);
			}
		}
		#endregion
	}
}
