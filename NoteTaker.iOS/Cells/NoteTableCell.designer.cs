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

namespace NoteTaker.iOS
{
    [Register ("NoteTableCell")]
    partial class NoteTableCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NoteDesctiptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NoteTimeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NoteTitleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (NoteDesctiptionLabel != null) {
                NoteDesctiptionLabel.Dispose ();
                NoteDesctiptionLabel = null;
            }

            if (NoteTimeLabel != null) {
                NoteTimeLabel.Dispose ();
                NoteTimeLabel = null;
            }

            if (NoteTitleLabel != null) {
                NoteTitleLabel.Dispose ();
                NoteTitleLabel = null;
            }
        }
    }
}