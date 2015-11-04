using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Splat;

namespace Xamalytics.Views
{
	public class TrackingButton : Button
	{
		readonly string _pageName;
		public TrackingButton (string pageName)
		{
			_pageName = pageName;
		}

		protected override void OnParentSet ()
		{
			base.OnParentSet ();

			if (Parent != null) {
				this.Clicked -= TrackingButton_Clicked;
				this.Clicked += TrackingButton_Clicked;
			}
			else
				this.Clicked -= TrackingButton_Clicked;
		}

		void TrackingButton_Clicked (object sender, EventArgs e)
		{
			Task.Run (() => {
				var analyticsServices = Splat.Locator.CurrentMutable.GetServices<Interfaces.IAnalytics> ();

				foreach (var analyticsService in analyticsServices)
					analyticsService.LogEvent (
						_pageName,
						"User Interaction",
						string.Format("{0} Clicked", Text),
						string.Empty
					);
			});
		}
	}
}

