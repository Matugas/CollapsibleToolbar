using System;
using CollapsibleToolbar.Helpers;
using CollapsibleToolbar.iOS.Helpers;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(KeyboardService))]
namespace CollapsibleToolbar.iOS.Helpers
{
    public class KeyboardService : IKeyboardService
    {
        public event EventHandler KeyboardIsShown;
        public event EventHandler KeyboardIsHidden;

        public KeyboardService()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            UIKeyboard.Notifications.ObserveWillShow(OnKeyboardDidShow);
            UIKeyboard.Notifications.ObserveWillHide(OnKeyboardDidHide);
        }

        private void OnKeyboardDidShow(object sender, EventArgs e)
        {
            KeyboardIsShown?.Invoke(this, EventArgs.Empty);
        }

        private void OnKeyboardDidHide(object sender, EventArgs e)
        {
            KeyboardIsHidden?.Invoke(this, EventArgs.Empty);
        }
    }
}