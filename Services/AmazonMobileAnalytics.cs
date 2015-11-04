using System;
using System.Collections.Generic;
using Amazon.MobileAnalytics.MobileAnalyticsManager;
using Amazon.Runtime;
using Amazon;
using Amazon.CognitoIdentity;

namespace Xamalytics.Services
{
	public class AmazonMobileAnalytics : Interfaces.IAnalytics
	{
		MobileAnalyticsManager _manager;

		string _amazonMobileAnalyticsAppId, _amazonCognitoIdentityPoolId;

		#region IAnalyticsService implementation

		bool _enabled;
		public bool Enabled {
			get {
				return _enabled;
			}
			set {
				_enabled = value;

				if (_manager != null) {
					if (_enabled)
						_manager.ResumeSession ();
					else
						_manager.PauseSession ();
				}
			}
		}

		public AmazonMobileAnalytics(string amazonMobileAnalyticsAppId, string amazonCognitoIdentityPoolId) {
			_amazonMobileAnalyticsAppId = amazonMobileAnalyticsAppId;
			_amazonCognitoIdentityPoolId = amazonCognitoIdentityPoolId;
		}

		public void Init ()
		{
			var config = new MobileAnalyticsManagerConfig();
			config.AllowUseDataNetwork = true; // Override the default Mobile Analytics Manager configuration if you need.

			_manager = MobileAnalyticsManager.GetOrCreateInstance(_amazonMobileAnalyticsAppId, //Amazon Mobile Analytics App ID
				new CognitoAWSCredentials(
					_amazonCognitoIdentityPoolId, //Amazon Cognito Identity Pool ID
					RegionEndpoint.USEast1),
				RegionEndpoint.USEast1, 
				config);

			_manager.ResumeSession ();

			Enabled = true;
		}

		public void SetUser (string identifier, Dictionary<string, string> userProperties)
		{
			//Not supported really, so we will not do anything here
		}

		public void LogViewLoaded (string name)
		{
			if (Enabled && _manager != null) {
				var loggingEvent = new CustomEvent (Constants.Tracking.ViewLoaded);
				loggingEvent.AddAttribute (Constants.Tracking.ViewName, name);
				_manager.RecordEvent (loggingEvent);
			}
		}

		public void LogViewUnloaded (string name)
		{
			if (Enabled && _manager != null) {
				var loggingEvent = new CustomEvent (Constants.Tracking.ViewUnloaded);
				loggingEvent.AddAttribute (Constants.Tracking.ViewName, name);
				_manager.RecordEvent (loggingEvent);
			}
		}

		public void LogViewDisplayed (string name)
		{
			if (Enabled && _manager != null) {
				var loggingEvent = new CustomEvent (Constants.Tracking.ViewDisplayed);
				loggingEvent.AddAttribute (Constants.Tracking.ViewName, name);
				_manager.RecordEvent (loggingEvent);
			}
		}

		public void LogViewHidden (string name)
		{
			if (Enabled && _manager != null) {
				var loggingEvent = new CustomEvent (Constants.Tracking.ViewHidden);
				loggingEvent.AddAttribute (Constants.Tracking.ViewName, name);
				_manager.RecordEvent (loggingEvent);
			}
		}

		public void LogError (string name, string title, string message){
			if (Enabled && _manager != null) {
				var loggingEvent = new CustomEvent (Constants.Tracking.UserError);
				loggingEvent.AddAttribute (Constants.Tracking.UserErrorView, name);
				loggingEvent.AddAttribute (Constants.Tracking.UserErrorTitle, title);
				loggingEvent.AddAttribute (Constants.Tracking.UserErrorMessage, message);
				_manager.RecordEvent (loggingEvent);
			}
		}

		public void LogEvent (string name, string category, string eventName, string eventData){
			if (Enabled && _manager != null) {
				var loggingEvent = new CustomEvent (name);
				loggingEvent.AddAttribute (Constants.Tracking.Category, category);
				loggingEvent.AddAttribute (Constants.Tracking.EventName, eventName);
				loggingEvent.AddAttribute (Constants.Tracking.EventData, eventData);
				_manager.RecordEvent (loggingEvent);
			}
		}

		public void LogPerformance (string category, TimeSpan performanceCounter, Dictionary<string, string> values = null)
		{
			if (Enabled && _manager != null) {
				if (values == null)
					values = new Dictionary<string, string> ();

				var loggingEvent = new CustomEvent (String.Format("Performance - {0}", category));
				loggingEvent.AddMetric ("Duration", performanceCounter.TotalMilliseconds);

				foreach (var value in values)
					loggingEvent.AddAttribute (value.Key, value.Value);

				_manager.RecordEvent (loggingEvent);
			}
		}

		#endregion
	}
}

