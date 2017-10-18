using Foundation;
using System;
using UIKit;
using ZXing.Mobile;

namespace BAAR.iOS
{
    public partial class ButtonPage : UIViewController
    {
        public ButtonPage(IntPtr handle) : base(handle)
        {
        }

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void UIButton985_TouchUpInside(UIButton sender)
        {
        }
    }
}