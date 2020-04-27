using Android.App;
using CollapsibleToolbar.Droid.Helpers;

namespace CollapsibleToolbar.Droid
{
    public static class Control
    {
        public static void Init(Activity mainActivity)
        {
            StatusBar.Activity = mainActivity;
            KeyboardService.Activity = mainActivity;
        }
    }
}
