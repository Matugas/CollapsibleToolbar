using System;
using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using CollapsibleToolbar.Droid.Helpers;
using CollapsibleToolbar.Helpers;
using static Android.Views.ViewTreeObserver;

[assembly: Xamarin.Forms.Dependency(typeof(KeyboardService))]
namespace CollapsibleToolbar.Droid.Helpers
{
    public class KeyboardService : Java.Lang.Object, IKeyboardService, IOnGlobalLayoutListener
    {
        public event EventHandler KeyboardIsShown;
        public event EventHandler KeyboardIsHidden;

        private InputMethodManager InputMethodManager;

        public static Activity Activity { get; set; }

        private bool wasShown = false;

        public KeyboardService()
        {
            GetInputMethodManager();
            SubscribeEvents();
        }

        public void OnGlobalLayout()
        {
            GetInputMethodManager();
            if (!wasShown && IsCurrentlyShown())
            {
                KeyboardIsShown?.Invoke(this, EventArgs.Empty);
                wasShown = true;
            }
            else if (wasShown && !IsCurrentlyShown())
            {
                KeyboardIsHidden?.Invoke(this, EventArgs.Empty);
                wasShown = false;
            }
        }

        private bool IsCurrentlyShown()
        {
            return InputMethodManager.IsAcceptingText;
        }

        private void GetInputMethodManager()
        {
            if (InputMethodManager == null || InputMethodManager.Handle == IntPtr.Zero)
            {
                InputMethodManager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            }
        }

        private void SubscribeEvents()
        {
            Activity.Window.DecorView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
        }
    }
}
