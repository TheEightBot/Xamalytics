using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Splat;

namespace Xamalytics.Pages
{
	public class NextPage : ContentPageBase
	{
		Label _nextPageTitle;

		ScrollView _mainScroll;

		StackLayout _mainStack;

		DateTimeOffset _displayTimeStart;

		public NextPage () : base("Next")
		{
			_mainScroll = new ScrollView {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			this.Content = _mainScroll;

			_mainStack = new StackLayout{ 
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			_mainScroll.Content = _mainStack;

			_nextPageTitle = new Label {
				Text = "Next"
			};
			_mainStack.Children.Add (_nextPageTitle);
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			_displayTimeStart = DateTimeOffset.Now;
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			Task.Run (() => {
				var analyticsServices = Splat.Locator.CurrentMutable.GetServices<Interfaces.IAnalytics> ();

				foreach (var analyticsService in analyticsServices)
					analyticsService.LogPerformance (
						"Next Page Display Time",
						DateTimeOffset.Now - _displayTimeStart
					);
			});
		}
	}
}

