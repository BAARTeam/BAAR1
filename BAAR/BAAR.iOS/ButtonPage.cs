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
            StartBarcode.TouchUpInside += async delegate
            {
               // MobileBarcodeScanner.Initialize();
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

    }
}