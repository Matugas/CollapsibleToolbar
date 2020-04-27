using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using CollapsibleToolbar.Droid.Helpers;
using CollapsibleToolbar.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(StatusBar))]
namespace CollapsibleToolbar.Droid.Helpers
{
    public class StatusBar : IStatusBar
    {
        public static Activity Activity { get; set; }

        public int GetHeight()
        {
            int statusBarHeight = -1;
            int resourceId = Activity.Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                statusBarHeight = Activity.Resources.GetDimensionPixelSize(resourceId);
            }
            return statusBarHeight;
        }

        public int BottomBarHeight { get; set; }

        public int GetBottomHeight()
        {
            int actionBarHeight = 0;

            if (HasSoftKeys())
            {
                actionBarHeight = BottomBarHeight;
            }
            return actionBarHeight;
        }



        public bool HasSoftKeys()
        {
            bool hasSoftwareKeys;
            var c = Android.App.Application.Context;

            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.JellyBeanMr1)
            {
                Display d = c.GetSystemService(Context.WindowService).JavaCast<IWindowManager>().DefaultDisplay;

                DisplayMetrics realDisplayMetrics = new DisplayMetrics();
                d.GetRealMetrics(realDisplayMetrics);

                int realHeight = realDisplayMetrics.HeightPixels;
                int realWidth = realDisplayMetrics.WidthPixels;

                DisplayMetrics displayMetrics = new DisplayMetrics();
                d.GetMetrics(displayMetrics);

                int displayHeight = displayMetrics.HeightPixels;
                int displayWidth = displayMetrics.WidthPixels;

                BottomBarHeight = (realHeight - displayHeight);

                hasSoftwareKeys = (realWidth - displayWidth) > 0 ||
                                   (realHeight - displayHeight) > 0;
            }
            else
            {
                bool hasMenuKey = ViewConfiguration.Get(c).HasPermanentMenuKey;
                bool hasBackKey = KeyCharacterMap.DeviceHasKey(Keycode.Back);
                hasSoftwareKeys = !hasMenuKey && !hasBackKey;
            }
            return hasSoftwareKeys;
        }
    }
}
