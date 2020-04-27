using Android.Content;
using Android.Views;
using CollapsibleToolbar.Droid.Helpers;
using CollapsibleToolbar.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyScrollView), typeof(MyScrollViewRenderer))]
namespace CollapsibleToolbar.Droid.Helpers
{
    public class MyScrollViewRenderer : ScrollViewRenderer
    {
        public MyScrollViewRenderer(Context context) : base(context)
        {
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            switch (ev.Action)
            {
                case MotionEventActions.Down:
                    (Element as MyScrollView).OnTouchDown();
                    break;
            }
            return base.OnInterceptTouchEvent(ev);
        }

        public override bool OnTouchEvent(MotionEvent ev)
        {
            switch (ev.Action)
            {
                case MotionEventActions.Down:
                    (Element as MyScrollView).OnTouchDown();
                    break;
                case MotionEventActions.Up:
                    (Element as MyScrollView).OnTouchUp();
                    break;
            }
            return base.OnTouchEvent(ev);
        }
    }
}
