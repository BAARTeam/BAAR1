// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace BAAR.iOS
{
    [Register ("StudentAccount")]
    partial class StudentAccount
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView Scroll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SubmitButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TicketButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView TicketHolder { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Scroll != null) {
                Scroll.Dispose ();
                Scroll = null;
            }

            if (SubmitButton != null) {
                SubmitButton.Dispose ();
                SubmitButton = null;
            }

            if (TicketButton != null) {
                TicketButton.Dispose ();
                TicketButton = null;
            }

            if (TicketHolder != null) {
                TicketHolder.Dispose ();
                TicketHolder = null;
            }
        }
    }
}