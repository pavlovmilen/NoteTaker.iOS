using NoteTaker.Core.Models;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NoteTaker.Core.Services;

namespace NoteTaker.Core.ViewModels
{
    public class NotesViewModel
    {
        public INoteStorageService NoteStorageService { get; private set; }
        public ObservableCollection<NoteEntryModel> Notes { get; set; } = new ObservableCollection<NoteEntryModel>();

        public NotesViewModel(INoteStorageService noteStorageService)
        {
            NoteStorageService = noteStorageService;
        }

        public async Task SetUp()
        {
            Notes = await NoteStorageService.SetUp();
        }

        public async Task<bool> AddEntry(NoteEntryModel entry)
        {
            return await NoteStorageService.AddOrUpdateNote(entry);
        }

        public async Task<bool> RemoveEntry(NoteEntryModel entry)
        {
            return await NoteStorageService.RemoveNote(entry);
        }

        private async Task<List<NoteEntryModel>> GetNotes()
        {
            return await NoteStorageService.GetNotes();
        }
    }
}
