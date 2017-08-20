using System;

namespace NoteTaker.Core.Models
{
    public class NoteEntryModel
    {
        public NoteEntryModel()
        {
            CreatedOn = DateTime.UtcNow;
            Title = "New note";
            Text = "No additional text";
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime?  CompletedOn { get; set; }

        public bool IsCompleted { get; set; }

    }
}
