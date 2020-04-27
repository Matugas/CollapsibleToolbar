using CollapsibleToolbar.Helpers;
using CollapsibleToolbar.iOS.Helpers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyScrollView), typeof(MyScrollViewRenderer))]
namespace CollapsibleToolbar.iOS.Helpers
{
    public class MyScrollViewRenderer : ScrollViewRenderer
    {
        public MyScrollViewRenderer()
        {
            this.DraggingStarted += (s, e) =>
            {
                (Element as MyScrollView).OnTouchDown();
            };

            this.DraggingEnded += (s, e) =>
            {
                (Element as MyScrollView).OnTouchUp();
            };
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            (Element as MyScrollView).OnTouchDown();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            (Element as MyScrollView).OnTouchUp();
        }
    }
}
