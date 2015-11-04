using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Splat;

namespace Xamalytics.Pages
{
	public class Login : ContentPageBase
	{
		Label _usernameHeader, _passwordHeader;

		Entry _username, _password;

		Button _login;

		ScrollView _mainScroll;

		StackLayout _mainStack;

		public Login () : base("Login")
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

			_usernameHeader = new Label {
				Text = "Username"
			};
			_mainStack.Children.Add (_usernameHeader);

			_username = new Entry { };
			_mainStack.Children.Add (_username);

			_passwordHeader = new Label {
				Text = "Password"
			};
			_mainStack.Children.Add (_passwordHeader);

			_password = new Entry { 
				IsPassword = true
			};
			_mainStack.Children.Add (_password);

			_login = new Button { 
				Text = "Login"
			};
			_login.Clicked += Login_Clicked;

			_mainStack.Children.Add (_login);
		}

		void Login_Clicked (object sender, EventArgs e)
		{
			Task.Run (() => {
				var analyticsServices = Splat.Locator.CurrentMutable.GetServices<Interfaces.IAnalytics> ();

				foreach (var analyticsService in analyticsServices)
					analyticsService.LogEvent (
						this.Title,
						"User Interaction",
						"Login Attempted",
						string.Empty
					);
			});

			this.Navigation.PushAsync (new HomePage ());
		}
	}
}

