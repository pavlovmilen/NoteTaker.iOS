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
        public readonly INoteStorageService _noteStorageService;
        public ObservableCollection<NoteEntryModel> Notes { get; set; } = new ObservableCollection<NoteEntryModel>();

        public NotesViewModel(INoteStorageService noteStorageService)
        {
            _noteStorageService = noteStorageService;
        }

        public async Task SetUp()
        {
            Notes = await _noteStorageService.SetUp();
        }

        public async Task<bool> AddEntry(NoteEntryModel entry)
        {
            return await _noteStorageService.AddOrUpdateNote(entry);
        }

        public async Task<bool> RemoveEntry(NoteEntryModel entry)
        {
            return await _noteStorageService.RemoveNote(entry);
        }

        private async Task<List<NoteEntryModel>> GetNotes()
        {
            return await _noteStorageService.GetNotes();
        }
    }
}
