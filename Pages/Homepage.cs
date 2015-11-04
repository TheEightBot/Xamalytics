using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Splat;

namespace Xamalytics.Pages
{
	public class HomePage : ContentPageBase
	{
		Label _homepageTitle;

		Views.TrackingButton _next;

		ScrollView _mainScroll;

		StackLayout _mainStack;

		public HomePage () : base("HomePage")
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

			_homepageTitle = new Label {
				Text = "Welcome"
			};
			_mainStack.Children.Add (_homepageTitle);

			_next = new Views.TrackingButton(this.Title) { 
				Text = "Next",
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			_next.Clicked += Next_Clicked;

			_mainStack.Children.Add (_next);
		}

		void Next_Clicked (object sender, EventArgs e)
		{
			this.Navigation.PushAsync (new NextPage ());
		}
	}
}

