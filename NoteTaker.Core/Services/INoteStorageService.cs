﻿using NoteTaker.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NoteTaker.Core.Services
{
    public interface INoteStorageService
    {
        Task<ObservableCollection<NoteEntryModel>> SetUp();

        Task<List<NoteEntryModel>> GetNotes();

        Task<bool> AddNote(NoteEntryModel note);

        Task<bool> RemoveNote(NoteEntryModel note);
    }
}
