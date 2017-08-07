using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteTaker.Core.Models
{
    public class NoteEntryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime?  CompletedOn { get; set; }

        public bool IsCompleted { get; set; }

    }
}
