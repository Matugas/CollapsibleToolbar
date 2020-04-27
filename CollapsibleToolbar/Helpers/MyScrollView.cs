using System;
using Xamarin.Forms;

namespace CollapsibleToolbar.Helpers
{
    public class MyScrollView : ScrollView
    {
        public event EventHandler TouchDown;
        public event EventHandler TouchUp;

        public void OnTouchDown() =>
            TouchDown?.Invoke(this, null);

        public void OnTouchUp() =>
            TouchUp?.Invoke(this, null);
    }
}
