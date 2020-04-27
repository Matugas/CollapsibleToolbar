using System;
using Xamarin.Forms;

namespace CollapsibleToolbar.Controls
{
    public partial class NavBarItemView : ContentView
	{
        public NavBarItemView()
        {
            InitializeComponent();
        }

        public void SetTapped(EventHandler tapped)
        {
            searchTapGR.Tapped += tapped;
        }
    }
}
