using Xamarin.Forms;

namespace CollapsibleToolbar.Controls
{
    public class NavBarItem : ToolbarItem
    {
        static readonly BindableProperty IsSearchProperty =
            BindableProperty.Create(nameof(IsSearch), typeof(bool), typeof(NavBarItem), false);

        public bool IsSearch
        {
            get { return (bool)GetValue(IsSearchProperty); }
            set { SetValue(IsSearchProperty, value); }
        }
    }
}
