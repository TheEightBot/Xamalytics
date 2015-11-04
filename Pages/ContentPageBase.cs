using System;
using Xamarin.Forms;
using Splat;
using System.Threading.Tasks;

namespace Xamalytics.Pages
{
	public abstract class ContentPageBase : ContentPage
	{
		readonly string _pageName;

		public ContentPageBase (string pageName)
		{
			if (string.IsNullOrEmpty (pageName))
				throw new ArgumentException ("A page name must be provided");

			Title = _pageName = pageName;
		}

		protected override void OnParentSet ()
		{
			base.OnParentSet ();

			Task.Run (() => {
				var hasParent = this.Parent != null;

				var analyticsServices = Splat.Locator.CurrentMutable.GetServices<Interfaces.IAnalytics> ();

				foreach (var analyticsService in analyticsServices){
					if(hasParent)
						analyticsService.LogViewLoaded (_pageName);
					else
						analyticsService.LogViewUnloaded (_pageName);
				}
			});
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			Task.Run (() => {
				var analyticsServices = Splat.Locator.Current.GetServices<Interfaces.IAnalytics> ();
		
				foreach (var analyticsService in analyticsServices)
					analyticsService.LogViewDisplayed (_pageName);
			});
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			Task.Run (() => {
				var analyticsServices = Splat.Locator.CurrentMutable.GetServices<Interfaces.IAnalytics> ();

				foreach (var analyticsService in analyticsServices)
					analyticsService.LogViewHidden (_pageName);
			});
		}
	}
}

