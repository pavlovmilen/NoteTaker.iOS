// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NoteTaker.iOS.Cells
{
    [Register ("NoteCell")]
    partial class NoteCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelTittle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LabelDate != null) {
                LabelDate.Dispose ();
                LabelDate = null;
            }

            if (LabelDescription != null) {
                LabelDescription.Dispose ();
                LabelDescription = null;
            }

            if (LabelTittle != null) {
                LabelTittle.Dispose ();
                LabelTittle = null;
            }
        }
    }
}