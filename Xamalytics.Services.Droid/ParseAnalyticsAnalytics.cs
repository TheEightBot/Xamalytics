using System;
using System.Collections.Generic;
using Parse;

namespace Xamalytics.Services.Droid
{
	public class ParseAnalyticsAnalytics : Interfaces.IAnalytics
	{

		string _applicationId, _dotNetKey;

		public ParseAnalyticsAnalytics(string applicationId, string dotNetKey){
			_applicationId = applicationId;
			_dotNetKey = dotNetKey;
		}

		#region IAnalytics implementation
		public bool Enabled {
			get;
			set;
		}

		public void Init ()
		{
			ParseClient.Initialize(_applicationId, _dotNetKey);

			Enabled = true;
		}

		/// <remarks>Google is super not cool with tracking people with our own custom information...</remarks>
		public void SetUser (string identifier, Dictionary<string, string> userProperties)
		{
			
		}

		public async void LogViewLoaded (string name)
		{
			if (Enabled)
				await ParseAnalytics.TrackEventAsync (
					Constants.Tracking.ViewLoaded.Replace(" ", "_"),
					new Dictionary<string, string>(){
						{ Constants.Tracking.ViewName, name }
					}
				)
					.ConfigureAwait(false);
		}

		public async void LogViewUnloaded (string name)
		{
			if (Enabled)
				await ParseAnalytics.TrackEventAsync (
					Constants.Tracking.ViewUnloaded.Replace(" ", "_"),
					new Dictionary<string, string>(){
						{ Constants.Tracking.ViewName, name }
					}
				)
					.ConfigureAwait(false);
		}

		public async void LogViewDisplayed (string name)
		{
			if (Enabled)
				await ParseAnalytics.TrackEventAsync (
					Constants.Tracking.ViewDisplayed.Replace(" ", "_"),
					new Dictionary<string, string>(){
						{ Constants.Tracking.ViewName, name }
					}
				)
					.ConfigureAwait(false);
		}

		public async void LogViewHidden (string name)
		{
			if (Enabled)
				await ParseAnalytics.TrackEventAsync (
					Constants.Tracking.ViewHidden.Replace(" ", "_"),
					new Dictionary<string, string>(){
						{ Constants.Tracking.ViewName, name }
					}
				)
					.ConfigureAwait(false);
		}

		public async void LogError (string name, string title, string message){
			if (Enabled)
				await ParseAnalytics.TrackEventAsync (
					Constants.Tracking.UserError.Replace(" ", "_"),
					new Dictionary<string, string>(){
						{ Constants.Tracking.UserErrorView, name },
						{ Constants.Tracking.UserErrorTitle, title },
						{ Constants.Tracking.UserErrorMessage, message }
					}
				)
					.ConfigureAwait(false);
		}

		public async void LogEvent (string name, string category, string eventName, string eventData){
			if (Enabled)
				await ParseAnalytics.TrackEventAsync (
					name.Replace(" ", "_"),
					new Dictionary<string, string>(){
						{ Constants.Tracking.Category, category },
						{ Constants.Tracking.EventName, eventName },
						{ Constants.Tracking.EventData, eventData },
					}
				)
					.ConfigureAwait(false);
		}

		public async void LogPerformance (string category, TimeSpan performanceCounter, Dictionary<string, string> values = null)
		{
			if (values == null)
				values = new Dictionary<string, string> ();

			values ["Duration"] = ((long)performanceCounter.TotalMilliseconds).ToString ();

			if (Enabled)
				await ParseAnalytics.TrackEventAsync (
					String.Format("Performance - {0}", category).Replace(" ", "_"),
					values
				)
					.ConfigureAwait(false);
		}
		#endregion
	}
}
