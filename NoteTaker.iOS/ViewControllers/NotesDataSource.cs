using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acr.UserDialogs;
using Foundation;
using NoteTaker.Core.Enums;
using NoteTaker.Core.Models;
using NoteTaker.Core.Services;
using UIKit;

namespace NoteTaker.iOS
{
    public class NotesDataSource : UITableViewSource
    {
        private static readonly NSString CellIdentifier = new NSString("NoteTableCell");
        private readonly MasterViewController _controller;
        private readonly INoteStorageService _noteStorageService;

        public NotesDataSource(MasterViewController controller, IList<NoteEntryModel> notes, INoteStorageService noteStorageService)
        {
            this._controller = controller;
            _noteStorageService = noteStorageService;
            Notes = new ObservableCollection<NoteEntryModel>(notes);
            // _controller.DetailViewController.OnEntityChanged = OnEntryAddedOrUpdated;
        }

        public ObservableCollection<NoteEntryModel> Notes { get; set; }

        // Customize the number of sections in the table view.
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Notes.Count;
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

        public override async void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            if (editingStyle == UITableViewCellEditingStyle.Delete)
            {
                // Delete the row from the data source.
                var existingNote = Notes[indexPath.Row];
                Notes.Remove(existingNote);
                _controller.TableView.DeleteRows(new[] { indexPath }, UITableViewRowAnimation.Fade);
                var removeNote = _noteStorageService?.RemoveNote(existingNote);
                var result = removeNote != null && await removeNote;

                if (result)
                {
                    UserDialogs.Instance.Toast("Note deleted");
                }
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

        public void OnEntryAddedOrUpdated(NoteEntryModel noteEntryModel, NoteOperationType noteOperationType)
        {
            if(noteOperationType == NoteOperationType.AddOrEdit)
            {
                var existingNote = Notes.FirstOrDefault(n => n.Id == noteEntryModel.Id);

                if (existingNote != null)
                {
                    Notes.Remove(existingNote);
                }

                Notes.Add(noteEntryModel);
            }

            else if (noteOperationType == NoteOperationType.Delete)
            {
                var existingNote = Notes.FirstOrDefault(n => n.Id == noteEntryModel.Id);

                if (existingNote != null)
                {
                    Notes.Remove(existingNote);
                }
            }

            _controller.TableView.ReloadData();
        }
    }
}