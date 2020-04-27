using System;
using System.Collections.Generic;
using CollapsibleToolbar.Controls;
using CollapsibleToolbar.ViewModels;
using Xamarin.Forms;

namespace CollapsibleToolbar
{
    public partial class FirstPage : CollapsiblePage
    {
        public FirstPage()
        {
            InitializeComponent();

            BindingContext = new FirstViewModel();

            picker.ItemsSource = new List<string>
            {
                "Escolha um dono", "Matugas", "Eduardo", "Ricardo"
            };
            picker.SelectedIndex = 0;

            slider.ValueChanged += Slider_ValueChanged;
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            sliderLabel.Text = Math.Round(slider.Value).ToString();
        }
    }
}
