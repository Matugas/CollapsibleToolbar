using System;
using System.Collections.Generic;
using CollapsibleToolbar.Controls;
using CollapsibleToolbar.ViewModels;

namespace CollapsibleToolbar
{
    public partial class ThirdPage : CollapsiblePage
    {
        public ThirdPage()
        {
            InitializeComponent();

            //NavBarItems = new List<NavBarItem>
            //{
            //    new NavBarItem
            //    {
            //        Icon = "playlist_edit"
            //    }
            //};

            BindingContext = new ThirdViewModel();
        }

        public void EntryFocused(object o, EventArgs e)
        {

        }
    }
}
