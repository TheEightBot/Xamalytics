using System;
using Android.Gms.Analytics;
using Android.Content;
using System.Collections.Generic;

namespace Xamalytics.Services.Droid
{
	public class GoogleAnalyticsAnalytics : Xamalytics.Interfaces.IAnalytics
	{
		private GoogleAnalytics _googleAnalytics;
		private Tracker _tracker;
		private Context _context;
		private string _trackingId;

		public GoogleAnalyticsAnalytics(Context context, string trackingId){
			_context = context;
			_trackingId = trackingId;
		}

		#region IAnalytics implementation
		public bool Enabled {
			get;
			set;
		}

		public void Init ()
		{
			_googleAnalytics = GoogleAnalytics.GetInstance (_context);

			if (_googleAnalytics != null) {
				
				// Optional: set Google Analytics dispatch interval to e.g. 20 seconds.
				_googleAnalytics.SetLocalDispatchPeriod(20);

				if (System.Diagnostics.Debugger.IsAttached)
					_googleAnalytics.Logger.LogLevel = Android.Gms.Analytics.LoggerLogLevel.Verbose;

				_tracker = _googleAnalytics.NewTracker (_trackingId);
			}

			Enabled = _googleAnalytics.IsInitialized && _tracker != null;
		}

		/// <remarks>Google is super not cool with tracking people with our own custom information...</remarks>
		public void SetUser (string identifier, Dictionary<string, string> userProperties)
		{
		}

		public void LogViewLoaded (string name)
		{
			if (Enabled && _tracker != null) {
				_tracker.SetScreenName (name);

				_tracker.Send (new HitBuilders.ScreenViewBuilder ().Build ());
				_tracker.Send (new HitBuilders
					.EventBuilder ()
					.SetCategory (Constants.Tracking.UserInterfaceEvent)
					.SetAction(Constants.Tracking.ViewDisplayed)
					.SetLabel(name)
					.Build());
				_tracker.SetScreenName (null);
			}
		}

		public void LogViewUnloaded (string name)
		{
			if (Enabled && _tracker != null) {
				_tracker.SetScreenName (name);

				_tracker.Send (new HitBuilders
					.EventBuilder ()
					.SetCategory (Constants.Tracking.UserInterfaceEvent)
					.SetAction(Constants.Tracking.ViewUnloaded)
					.SetLabel(name)
					.Build());
				_tracker.SetScreenName (null);
			}
		}

		public void LogViewDisplayed (string name)
		{
			if (Enabled && _tracker != null) {
				_tracker.SetScreenName (name);

				_tracker.Send (new HitBuilders
					.EventBuilder ()
					.SetCategory (Constants.Tracking.UserInterfaceEvent)
					.SetAction(Constants.Tracking.ViewDisplayed)
					.SetLabel(name)
					.Build());
				_tracker.SetScreenName (null);
			}
		}

		public void LogViewHidden (string name)
		{
			if (Enabled && _tracker != null) {
				_tracker.SetScreenName (name);

				_tracker.Send (new HitBuilders
					.EventBuilder ()
					.SetCategory (Constants.Tracking.UserInterfaceEvent)
					.SetAction(Constants.Tracking.ViewHidden)
					.SetLabel(name)
					.Build());
				_tracker.SetScreenName (null);
			}
		}

		public void LogError (string name, string title, string message)
		{
			if (Enabled && _tracker != null) {
				_tracker.SetScreenName (name);

				_tracker.Send (new HitBuilders.ExceptionBuilder()
					.SetDescription(string.Format("name: {0}\ttitle: {1}\tmessage: {2}", name, title, message))
					.SetFatal(false)
					.Build());
				_tracker.SetScreenName (null);
			}
		}

		public void LogEvent (string name, string category, string eventName, string eventData){
			if (Enabled && _tracker != null) {
				_tracker.SetScreenName (name);

				_tracker.Send (new HitBuilders
					.EventBuilder ()
					.SetCategory (category)
					.SetAction(eventName)
					.SetLabel(eventData)
					.Build());
				_tracker.SetScreenName (null);
			}
		}

		public void LogPerformance (string category, TimeSpan performanceCounter, Dictionary<string, string> values = null)
		{
			if (Enabled && _tracker != null) {
				_tracker.Send (new HitBuilders
					.TimingBuilder ()
					.SetCategory (category)
					.SetValue ((long)performanceCounter.TotalMilliseconds)
					.Build());
			}
		}
		#endregion
	}
}

