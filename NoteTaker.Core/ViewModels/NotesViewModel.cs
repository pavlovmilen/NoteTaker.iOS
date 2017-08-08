using NoteTaker.Core.Models;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteTaker.Core.ViewModels
{
    public class NotesViewModel
    {
        public ObservableCollection<NoteEntryModel> Notes { get; set; }
        public NotesViewModel()
        {

        }

        public async void SetUp()
    }
}
