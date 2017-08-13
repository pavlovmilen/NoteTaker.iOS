using Foundation;
using NoteTaker.Core.Models;
using System;
using UIKit;

namespace NoteTaker.iOS
{
    public partial class NoteTableCell : UITableViewCell
    {
        public NoteTableCell (IntPtr handle) : base (handle)
        {
        }

        public void UpdateCell(NoteEntryModel model)
        {
            if (model != null)
            {
                NoteTimeLabel.Text = model.CreatedOn.ToString("dd/MM/yyyy");
                NoteTitleLabel.Text = model.Title;
                NoteDesctiptionLabel.Text = model.Text == null ? string.Empty : model.Text.Length > 60 ? model.Text?.Substring(0, 60) : model.Text;
            }
        }
    }
}