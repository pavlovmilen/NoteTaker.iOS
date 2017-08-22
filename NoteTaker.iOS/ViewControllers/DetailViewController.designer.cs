// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NoteTaker.iOS
{
    [Register ("DetailViewController")]
    partial class DetailViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AddImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DeleteImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch NoteCompletedSwitch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView NoteDescriptionTextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView NoteImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TitleTextView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AddImageButton != null) {
                AddImageButton.Dispose ();
                AddImageButton = null;
            }

            if (DeleteImageButton != null) {
                DeleteImageButton.Dispose ();
                DeleteImageButton = null;
            }

            if (NoteCompletedSwitch != null) {
                NoteCompletedSwitch.Dispose ();
                NoteCompletedSwitch = null;
            }

            if (NoteDescriptionTextView != null) {
                NoteDescriptionTextView.Dispose ();
                NoteDescriptionTextView = null;
            }

            if (NoteImageView != null) {
                NoteImageView.Dispose ();
                NoteImageView = null;
            }

            if (TitleTextView != null) {
                TitleTextView.Dispose ();
                TitleTextView = null;
            }
        }
    }
}