using Acr.UserDialogs;
using NoteTaker.Core.Models;
using NoteTaker.Core.Services;
using System;
using NoteTaker.Core.Enums;
using Plugin.Media;
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
                NoteCompletedSwitch.On = Note.IsCompleted;

                if (!string.IsNullOrEmpty(Note.ImagePath))
                {
                    ShowImage();
                    NoteImageView.Image = UIImage.FromFile(Note.ImagePath);
                }
            }
		}

	    private void SetControlsVisibility()
	    {
	        TitleTextView.Hidden = false;
	        NoteCompletedSwitch.Hidden = false;
	        NoteImageView.Hidden = DeleteImageButton.Hidden = !Note.HasImage;
	        AddImageButton.Hidden = Note.HasImage;
	    }

	    public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			ConfigureView ();

            var saveButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, OnSave) { AccessibilityLabel = "saveButton" };
            var deleteButton = new UIBarButtonItem(UIBarButtonSystemItem.Trash, OnDelete) { AccessibilityLabel = "deleteButton" };
            NavigationItem.RightBarButtonItems = new UIBarButtonItem[] { saveButton, deleteButton };

            TitleTextView.AddTarget(TitleTextViewOnValueChanged, UIControlEvent.EditingChanged);
            NoteDescriptionTextView.Changed += NoteDescriptionTextViewOnChanged;
            NoteCompletedSwitch.ValueChanged += NoteCompletedSwitchOnValueChanged;
            AddImageButton.TouchUpInside += AddImageButton_TouchUpInside;
            DeleteImageButton.TouchUpInside += DeleteImageButton_TouchUpInside;
		    if (Note == null)
		    {
		        TitleTextView.Hidden = NoteCompletedSwitch.Hidden = NoteImageView.Hidden = DeleteImageButton.Hidden = AddImageButton.Hidden = true;
		    }
		    else
		    {
		        SetControlsVisibility();
		    }
           
		}

        private void DeleteImageButton_TouchUpInside(object sender, EventArgs e)
        {
            Note.ImagePath = null;
            NoteImageView.Hidden = DeleteImageButton.Hidden = true;
            AddImageButton.Hidden = false;
        }

	    private void ShowImage()
	    {
	        NoteImageView.Hidden = DeleteImageButton.Hidden = false;
	        AddImageButton.Hidden = true;
        }

	    private async void AddImageButton_TouchUpInside(object sender, EventArgs e)
        {
            var pickerStarted = await CrossMedia.Current.Initialize();

            if (pickerStarted)
            {
                var picker = CrossMedia.Current;

                if (!picker.IsPickPhotoSupported)
                {
                    return;
                }

                var result = await picker.PickPhotoAsync();

                if (!string.IsNullOrEmpty(result?.Path))
                {
                    NoteImageView.Image = UIImage.FromFile(result.Path);
                    Note.ImagePath = result.Path;
                    ShowImage();
                }                
            }
        }

        private void NoteCompletedSwitchOnValueChanged(object sender, EventArgs eventArgs)
	    {
	        var completed = ((UISwitch) sender).On;
	        Note.IsCompleted = completed;
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


