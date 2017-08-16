using NoteTaker.Core.Models;
using NoteTaker.Core.Services;
using System;

using UIKit;

namespace NoteTaker.iOS
{
	public partial class DetailViewController : UIViewController
	{
        public INoteStorageService NoteStorageService { get; set; }

		public NoteEntryModel Note { get; set; }

		public DetailViewController (IntPtr handle) : base (handle)
		{
		}

		public void SetDetailItem (NoteEntryModel note)
		{
			if (Note != note)
            {
				Note = note;
				
				// Update the view
				ConfigureView ();
			}
		}

		void ConfigureView ()
		{
            // Update the user interface for the detail item
            if (IsViewLoaded && Note != null)
            {
                NoteDescriptionTextView.Text = Note.Text;
                TitleTextView.Text = Note.Title;
            }
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			ConfigureView ();

            var saveButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, OnSave) { AccessibilityLabel = "saveButton" };
            var deleteButton = new UIBarButtonItem(UIBarButtonSystemItem.Trash, OnDelete) { AccessibilityLabel = "deleteButton" };
            NavigationItem.RightBarButtonItems = new UIBarButtonItem[] { saveButton, deleteButton };
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            await NoteStorageService.RemoveNote(Note);
        }

        private async void OnSave(object sender, EventArgs e)
        {
            if (Note == null)
            {
                return;
            }

            var exisingNoteText = await NoteStorageService.NoteExists(Note);

            if (string.IsNullOrEmpty(exisingNoteText))
            {
                await NoteStorageService.AddNote(Note);
            }
            else
            {
                var alert = UIAlertController.Create("Warning", exisingNoteText, UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(alert, true, null);
            }

        }

        public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


