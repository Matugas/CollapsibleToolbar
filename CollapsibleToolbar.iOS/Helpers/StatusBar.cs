using CollapsibleToolbar.Helpers;
using CollapsibleToolbar.iOS.Helpers;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(StatusBar))]
namespace CollapsibleToolbar.iOS.Helpers
{
    public class StatusBar : IStatusBar
    {
        public int GetHeight()
        {
            return (int)UIApplication.SharedApplication.StatusBarFrame.Height;
        }

        public int GetBottomHeight()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                var insets = UIApplication.SharedApplication.Windows[0].SafeAreaInsets;
                return (int)insets.Bottom;
            }
            return 0;
        }
    }
}
