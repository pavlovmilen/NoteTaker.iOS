using System;
using System.Collections.Generic;

using UIKit;
using Foundation;
using NoteTaker.Core.Models;
using NoteTaker.Core.ViewModels;
using NoteTaker.Core.Services;

namespace NoteTaker.iOS
{
	public partial class MasterViewController : UITableViewController
	{
	    private NotesViewModel _viewModel;
		public DetailViewController DetailViewController { get; set; }

		DataSource dataSource;

		protected MasterViewController(IntPtr handle) : base(handle)
		{
            // Note: this .ctor should not contain any initialization logic.

            //TableView.RegisterClassForCellReuse(typeof(NoteCell), "NoteCell");
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();

			Title = NSBundle.MainBundle.LocalizedString("Master", "Master");

			// Perform any additional setup after loading the view, typically from a nib.
			NavigationItem.LeftBarButtonItem = EditButtonItem;

		    var addButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, AddNewItem) {AccessibilityLabel = "addButton"};
		    NavigationItem.RightBarButtonItem = addButton;
            _viewModel = AppDelegate.Container.Resolve(typeof(NotesViewModel), "notesViewModel") as NotesViewModel;
		    await _viewModel.SetUp();
			DetailViewController = (DetailViewController)((UINavigationController)SplitViewController.ViewControllers[1]).TopViewController;

			TableView.Source = dataSource = new DataSource(this, _viewModel);
		}

		public override void ViewWillAppear(bool animated)
		{
			ClearsSelectionOnViewWillAppear = SplitViewController.Collapsed;
			base.ViewWillAppear(animated);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		void AddNewItem(object sender, EventArgs args)
		{
			dataSource.Notes.Add(new NoteEntryModel());

			using (var indexPath = NSIndexPath.FromRowSection(0, 0))
				TableView.InsertRows(new[] { indexPath }, UITableViewRowAnimation.Automatic);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "showDetail")
			{
				var controller = (DetailViewController)((UINavigationController)segue.DestinationViewController).TopViewController;
				var indexPath = TableView.IndexPathForSelectedRow;
				var item = dataSource.Notes[indexPath.Row];

				controller.SetDetailItem(item);
				controller.NavigationItem.LeftBarButtonItem = SplitViewController.DisplayModeButtonItem;
				controller.NavigationItem.LeftItemsSupplementBackButton = true;
			}
		}

		class DataSource : UITableViewSource
		{
			static readonly NSString CellIdentifier = new NSString("NoteTableCell");
		    private readonly NotesViewModel _viewModel;
			readonly MasterViewController _controller;

			public DataSource(MasterViewController controller, NotesViewModel viewModel)
			{
			    this._controller = controller;
			    _viewModel = viewModel;
			}

			public IList<NoteEntryModel> Notes => _viewModel.Notes;

		    // Customize the number of sections in the table view.
			public override nint NumberOfSections(UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection(UITableView tableview, nint section)
			{
				return _viewModel.Notes.Count;
			}

			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell(CellIdentifier, indexPath) as NoteTableCell;

			    if (cell == null)
			    {
			        return null;
			    }

                
                cell.UpdateCell(Notes[indexPath.Row]);

				return cell;
			}

			public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
			{
				// Return false if you do not want the specified item to be editable.
				return true;
			}

			public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				if (editingStyle == UITableViewCellEditingStyle.Delete)
				{
					// Delete the row from the data source.
					_viewModel.Notes.RemoveAt(indexPath.Row);
					_controller.TableView.DeleteRows(new[] { indexPath }, UITableViewRowAnimation.Fade);
				}
				else if (editingStyle == UITableViewCellEditingStyle.Insert)
				{
					// Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
				}
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				_controller.DetailViewController.SetDetailItem(Notes[indexPath.Row]);
			}
		}
	}
}
