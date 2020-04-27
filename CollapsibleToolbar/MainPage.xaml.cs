using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace CollapsibleToolbar
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();

            btnFirstPage.Clicked += BtnFirstPage_Clicked;
            btnSecondPage.Clicked += BtnSecondPage_Clicked;
            btnThirdPage.Clicked += BtnThirdPage_Clicked;
        }

        private async void BtnFirstPage_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new FirstPage());
        }

        private async void BtnSecondPage_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SecondPage());
        }

        private async void BtnThirdPage_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ThirdPage());
        }
    }
}
