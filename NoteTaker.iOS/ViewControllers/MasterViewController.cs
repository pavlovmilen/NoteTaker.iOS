using System;
using System.Linq;
using UIKit;
using Foundation;
using NoteTaker.Core.Models;
using NoteTaker.Core.ViewModels;
using NoteTaker.Core.Services;
using CoreGraphics;

namespace NoteTaker.iOS
{
	public partial class MasterViewController : UITableViewController
	{
	    private NotesViewModel _viewModel;
		public DetailViewController DetailViewController { get; set; }

		NotesDataSource _dataSource;

		protected MasterViewController(IntPtr handle) : base(handle)
		{
            // Note: this .ctor should not contain any initialization logic.
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();

			Title = NSBundle.MainBundle.LocalizedString("Notes", "Notes");

        

			// Perform any additional setup after loading the view, typically from a nib.
			NavigationItem.LeftBarButtonItem = EditButtonItem;

		    var addButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, AddNewItem) {AccessibilityLabel = "addButton"};
		    NavigationItem.RightBarButtonItem = addButton;
            _viewModel = AppDelegate.Container.Resolve(typeof(NotesViewModel), "notesViewModel") as NotesViewModel;
		    await _viewModel?.SetUp();
			DetailViewController = (DetailViewController)((UINavigationController)SplitViewController.ViewControllers[1]).TopViewController;

            TableView.Source = _dataSource = new NotesDataSource(this, _viewModel.Notes, _viewModel.NoteStorageService);
            TableView.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
		{
			ClearsSelectionOnViewWillAppear = SplitViewController.Collapsed;
			base.ViewWillAppear(animated);

            TableView.SetContentOffset(new CGPoint(0,0), true);

		    var indexPath = NSIndexPath.FromRowSection(0, 0);
		    if (_viewModel.Notes.Any())
		    {
		        TableView.SelectRow(indexPath, true, UITableViewScrollPosition.Bottom);
		    }
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		void AddNewItem(object sender, EventArgs args)
		{
            int lastPosition = _dataSource.Notes.Count;
            _dataSource.Notes.Add(new NoteEntryModel());

            using (var indexPath = NSIndexPath.FromRowSection(lastPosition, 0))
				TableView.InsertRows(new[] { indexPath }, UITableViewRowAnimation.Automatic);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "showDetail")
			{
				var controller = (DetailViewController)((UINavigationController)segue.DestinationViewController).TopViewController;
				var indexPath = TableView.IndexPathForSelectedRow;
				var item = _dataSource.Notes[indexPath.Row];

				controller.SetDetailItem(item);
				controller.NavigationItem.LeftBarButtonItem = SplitViewController.DisplayModeButtonItem;
				controller.NavigationItem.LeftItemsSupplementBackButton = true;
                controller.NoteStorageService = _viewModel.NoteStorageService;

			    if (controller.OnEntityChanged == null)
			    {
			        controller.OnEntityChanged = _dataSource.OnEntryAddedOrUpdated;
			    }
			}
		}
	}
}
