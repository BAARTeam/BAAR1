// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace BAAR.iOS
{
    [Register ("ButtonPage")]
    partial class ButtonPage
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Buttons { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Scan { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Buttons != null) {
                Buttons.Dispose ();
                Buttons = null;
            }

            if (Scan != null) {
                Scan.Dispose ();
                Scan = null;
            }
        }
    }
}