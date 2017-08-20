﻿using Acr.UserDialogs;
using NoteTaker.Core.Models;
using NoteTaker.Core.Services;
using System;
using NoteTaker.Core.Enums;
using UIKit;

namespace NoteTaker.iOS
{
	public partial class DetailViewController : UIViewController
	{
        public INoteStorageService NoteStorageService { get; set; }

		public NoteEntryModel Note { get; set; }

        public Action<NoteEntryModel, NoteOperationType> OnEntityChanged { get; set; }

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

            TitleTextView.AddTarget(TitleTextViewOnValueChanged, UIControlEvent.EditingChanged);// += TitleTextViewOnValueChanged;
            NoteDescriptionTextView.Changed += NoteDescriptionTextViewOnChanged;
        }

	    private void NoteDescriptionTextViewOnChanged(object sender, EventArgs eventArgs)
	    {
	        Note.Text = ((UITextView)sender).Text;
        }

	    private void TitleTextViewOnValueChanged(object sender, EventArgs eventArgs)
	    {

	        Note.Title = ((UITextField) sender).Text;
	    }

	    private async void OnDelete(object sender, EventArgs e)
        {
            if (Note == null)
            {
                return;
            }

            var result = await NoteStorageService.RemoveNote(Note);

            if (result)
            {
                UserDialogs.Instance.Toast("Note deleted", TimeSpan.FromSeconds(3));
                //notify masterviewcontroller
                OnEntityChanged(Note, NoteOperationType.Delete);

                DismissKeyboard();
            }
            else
            {
                UserDialogs.Instance.Toast($"An error occurred while deleting note: {Note.Title}. Please try again");
            }
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
                var result = await NoteStorageService.AddOrUpdateNote(Note);

                if (result)
                {
                    UserDialogs.Instance.Toast("Note saved", TimeSpan.FromSeconds(3));
                    //notify masterviewcontroller
                    OnEntityChanged?.Invoke(Note, NoteOperationType.AddOrEdit);
                }

                DismissKeyboard();
            }
            else
            {
                UserDialogs.Instance.Toast($"Warning {exisingNoteText}.");
            }

        }

        public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

	    private void DismissKeyboard()
	    {
	        if (TitleTextView.CanResignFirstResponder)
	        {
	            TitleTextView.ResignFirstResponder();
	        }

	        if (NoteDescriptionTextView.CanResignFirstResponder)
	        {
	            NoteDescriptionTextView.ResignFirstResponder();
	        }
        }
	}
}


