using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Splat;
using System.Collections.Generic;

namespace Xamalytics.Pages
{
	public class NextPage : ContentPageBase
	{
		Label _nextPageTitle;

		ScrollView _mainScroll;

		StackLayout _mainStack;

		DateTimeOffset _displayTimeStart;

		Image _arcade, _playstation, _xbox;

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

			_arcade = new Image {
				Source = ImageSource.FromResource("Xamalytics.Images.play.png"),
				HorizontalOptions = LayoutOptions.Center
			};

			var arcadeTapped = new TapGestureRecognizer ();
			arcadeTapped.Tapped += async (sender, e) => PlatformSelected("Arcade");

			_arcade.GestureRecognizers.Add (arcadeTapped);
			_mainStack.Children.Add (_arcade);

			_playstation = new Image {
				Source = ImageSource.FromResource("Xamalytics.Images.dualshock.png"),
				HorizontalOptions = LayoutOptions.Center
			};

			var playstationTapped = new TapGestureRecognizer ();
			playstationTapped.Tapped += async (psender, pe) => PlatformSelected("Playstation");

			_playstation.GestureRecognizers.Add (playstationTapped);
			_mainStack.Children.Add (_playstation);

			_xbox = new Image {
				Source = ImageSource.FromResource("Xamalytics.Images.xbox-one.png"),
				HorizontalOptions = LayoutOptions.Center
			};

			var xboxTapped = new TapGestureRecognizer ();
			xboxTapped.Tapped += async (xsender, xe) => PlatformSelected("Xbox");

			_xbox.GestureRecognizers.Add (xboxTapped);
			_mainStack.Children.Add (_xbox);
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			_displayTimeStart = DateTimeOffset.Now;
		}

		async Task PlatformSelected(string platform){

			var decisionDelay = DateTimeOffset.Now - _displayTimeStart;

			Task.Run (() => {
				var analyticsServices = Locator.CurrentMutable.GetServices<Interfaces.IAnalytics> ();

				foreach (var analyticsService in analyticsServices) {
					analyticsService.LogPerformance (
						"Platform Selection",
						decisionDelay,
						new Dictionary<string, string>{
							{ "Platform", platform }
						}
					);

					analyticsService.LogEvent (
						"Game Platform Selected",
						"User Interaction",
						"Selected Console",
						platform
					);
				}
			});

			await DisplayAlert ("Platform Selected", platform + " was a great choice", "yep");

			await this.Navigation.PopAsync ();
		}
	}
}

