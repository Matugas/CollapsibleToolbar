using System;
using Xamarin.Forms;

namespace CollapsibleToolbar.Controls
{
    public partial class NavBar : ContentView
    {
        public string Title { get => lblTitle.Text; set => lblTitle.Text = value; }

        public double TitleOpacity { get
            {
                return lblTitle.Opacity;
            }
            set
            {
                lblTitle.Opacity = value;
                separator.Opacity = value;
            }
        }

        public bool HasBackButton { get => backBtn.IsVisible; set => backBtn.IsVisible = value; }

        public bool SearchBarIsVisible { get => searchBar.IsVisible; set => searchBar.IsVisible = value; }
        //public bool SearchBarIsVisible { get => searchBarContainer.IsVisible; set => searchBarContainer.IsVisible = value; }

        public double SearchBarScaleY { get => searchBar.ScaleY; set => searchBar.ScaleY = value; }

        public CollapsiblePage Page { get; set; }


        public NavBar()
        {
            InitializeComponent();

            SearchBarIsVisible = false;
        }

        public void AddItem(NavBarItem  item)
        {
            var nbItem = new NavBarItemView
            {
                BindingContext = item
            };

            if (item.IsSearch)
            {
                nbItem.SetTapped(SearchTapped);
                searchBar.Unfocused += (o, s) =>
                {
                    searchBar.SearchCommand.Execute(null);
                };
            }
            itemsContainer.Children.Add(nbItem);
        }

        private async void SearchTapped(object sender, EventArgs e)
        {
            Page.SearchBarIsVisible = !Page.SearchBarIsVisible;

            Animation animation;
            if (Page.SearchBarIsVisible)
            {
                animation = new Animation(y => Page.SearchBarScaleY = y, 0, 1);
            }
            else
            {
                animation = new Animation(y => Page.SearchBarScaleY = y, 1, 0);
            }

            animation.Commit(
                searchBar,
                "toggleSearchBar",
                length: 100,
                easing: Easing.SinIn,
                finished: (a, b) =>
                {
                    Page.CalculateFiller();
                }
            );
        }

        private async void OnBackPressed(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopAsync(true);
        }
    }
}
