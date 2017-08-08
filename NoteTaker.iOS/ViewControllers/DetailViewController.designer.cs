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
        UIKit.UITextView NoteDesctiptionTextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TitleTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (NoteDesctiptionTextView != null) {
                NoteDesctiptionTextView.Dispose ();
                NoteDesctiptionTextView = null;
            }

            if (TitleTextField != null) {
                TitleTextField.Dispose ();
                TitleTextField = null;
            }
        }
    }
}