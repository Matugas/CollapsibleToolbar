using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CollapsibleToolbar
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Color barBackgroundColor;
            Color barTextColor;
            if (Device.RuntimePlatform == Device.Android)
            {
                barTextColor = Color.Black;
                barBackgroundColor = Color.White;
            }
            else
            {
                barTextColor = Color.FromHex("#F1F2EB");
                barBackgroundColor = Color.FromHex("#7A542E");
            }

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = barBackgroundColor,
                BarTextColor = barTextColor
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
