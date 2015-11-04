using System;
using System.Collections.Generic;
using Segment;

namespace Xamalytics.Services
{
	public class SegmentAnalyticsAnalytics : Interfaces.IAnalytics
	{
		#region IAnalyticsService implementation

		public bool Enabled {
			get;
			set;
		}

		readonly string _apiKey;

		string _userIdentifier;

		public SegmentAnalyticsAnalytics(string apiKey) {
			_apiKey = apiKey;
			_userIdentifier = Guid.NewGuid ().ToString();
		}

		public void Init ()
		{
			Analytics.Initialize(_apiKey);
			Enabled = true;
		}

		public void SetUser (string identifier, Dictionary<string, string> userProperties)
		{
			if (Enabled) {
				var traits = new Segment.Model.Traits ();
				foreach (var userProperty in userProperties) {
					traits.Add (userProperty.Key, userProperty.Value);
				}

				Analytics.Client.Identify (identifier, traits);

				Analytics.Client.Alias (_userIdentifier, identifier);
				_userIdentifier = identifier;
			}
		}

		public void LogViewLoaded (string name)
		{
			if (Enabled) {
				var properties = new Segment.Model.Properties {
					{
						Constants.Tracking.UserInterfaceEvent,
						Constants.Tracking.ViewLoaded
					}
				};

				Analytics.Client.Screen (_userIdentifier, name, properties);
			}
		}

		public void LogViewUnloaded (string name)
		{
			if (Enabled) {
				var properties = new Segment.Model.Properties {
					{
						Constants.Tracking.UserInterfaceEvent,
						Constants.Tracking.ViewUnloaded
					}
				};

				Analytics.Client.Screen (_userIdentifier, name, properties);
			}
		}

		public void LogViewDisplayed (string name)
		{
			if (Enabled) {
				var properties = new Segment.Model.Properties {
					{
						Constants.Tracking.UserInterfaceEvent,
						Constants.Tracking.ViewDisplayed
					}
				};

				Analytics.Client.Screen (_userIdentifier, name, properties);
			}
		}

		public void LogViewHidden (string name)
		{
			if (Enabled) {
				var properties = new Segment.Model.Properties {
					{
						Constants.Tracking.UserInterfaceEvent,
						Constants.Tracking.ViewHidden
					}
				};

				Analytics.Client.Screen (_userIdentifier, name, properties);
			}
		}

		public void LogError (string name, string title, string message){
			if (Enabled) {
				Analytics.Client.Track(
					_userIdentifier,
					Constants.Tracking.UserError,
					new Segment.Model.Properties {
						{ Constants.Tracking.UserErrorView, name },
						{ Constants.Tracking.UserErrorTitle, title },
						{ Constants.Tracking.UserErrorMessage, message }
					});
			}
		}

		public void LogEvent (string name, string category, string eventName, string eventData){
			if (Enabled) {
				Analytics.Client.Track(
					_userIdentifier,
					category,
					new Segment.Model.Properties {
						{ Constants.Tracking.Category, category },
						{ Constants.Tracking.EventName, eventName },
						{ Constants.Tracking.EventData, eventData },
					});
			}
		}

		public void LogPerformance (string category, TimeSpan performanceCounter, Dictionary<string, string> values = null)
		{
			if (Enabled) {
				if (values == null)
					values = new Dictionary<string, string> ();

				var properties = new Segment.Model.Properties {
					{ Constants.Tracking.Category, category },
					{ "Duration", ((long)performanceCounter.TotalMilliseconds).ToString () }
				};

				foreach (var value in values) {
					properties.Add (value.Key, value.Value);
				}
					
				Analytics.Client.Track(
					_userIdentifier,
					Constants.Tracking.Performance,
					properties);
			}
		}

		#endregion
	}
}

