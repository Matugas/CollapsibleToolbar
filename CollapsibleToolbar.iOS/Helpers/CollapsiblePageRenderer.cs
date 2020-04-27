using CollapsibleToolbar.Controls;
using CollapsibleToolbar.iOS.Helpers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(CollapsiblePage), typeof(CollapsiblePageRenderer))]
namespace CollapsibleToolbar.iOS.Helpers
{
    public class CollapsiblePageRenderer : PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var element = (Element as CollapsiblePage);
            ViewController.NavigationController.InteractivePopGestureRecognizer.Enabled = element.HasBackButton;
            ViewController.NavigationController.InteractivePopGestureRecognizer.Delegate = new MyGRD();
        }
    }

    public class MyGRD : UIGestureRecognizerDelegate
    {
        public override bool ShouldBegin(UIGestureRecognizer recognizer)
        {
            return Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Count > 1;
        }
    }
}
