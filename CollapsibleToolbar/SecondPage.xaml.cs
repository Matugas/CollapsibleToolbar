using System;
using System.Diagnostics;
using CollapsibleToolbar.Controls;
using CollapsibleToolbar.ViewModels;

namespace CollapsibleToolbar
{
    public partial class SecondPage : CollapsiblePage
    {
        public SecondPage()
        {
            InitializeComponent();
            var viewModel = new SecondViewModel();
            BindingContext = viewModel;
        }

        void Tapps(object o, EventArgs e)
        {
            Debug.WriteLine("Coé:" + e);
        }
    }
}
