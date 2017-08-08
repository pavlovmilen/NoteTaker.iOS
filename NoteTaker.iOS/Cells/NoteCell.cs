using System;

using Foundation;
using UIKit;
using NoteTaker.Core.Models;

namespace NoteTaker.iOS.Cells
{
    public partial class NoteCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("NoteCell");
        public static readonly UINib Nib;

        static NoteCell()
        {
            Nib = UINib.FromName("NoteCell", NSBundle.MainBundle);
        }

        protected NoteCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void UpdateCell(NoteEntryModel model)
        {
            if (model != null)
            {
                LabelTittle.Text = model.Title;
                LabelDate.Text = model.CreatedOn.ToString("dd/MM/yyyy");
                LabelDescription.Text = model.Text.Substring(0, 30) + ".....";
            }
        }
    }
}
