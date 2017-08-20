using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteTaker.Core.Models;
using PCLStorage;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Acr.UserDialogs;

namespace NoteTaker.Core.Services
{
    public class NoteStorageService : INoteStorageService
    {
        private readonly IFolder _rootFolder;

        public NoteStorageService()
        {
            _rootFolder = FileSystem.Current.LocalStorage;
        }

        public async Task<bool> AddOrUpdateNote(NoteEntryModel note)
        {
            var notes = await GetNotes() ?? new List<NoteEntryModel>();

            if (note.Id == 0)
            {
                SetNoteId(notes, note);
            }
            else
            {
                var existingNote = notes.FirstOrDefault(n => n.Id == note.Id);
                if (existingNote != null)
                {
                    notes.Remove(existingNote);
                }
            }

            notes.Add(note);

            return await SaveNotes(notes);
        }

        private void SetNoteId(List<NoteEntryModel> notes, NoteEntryModel note)
        {
            if (notes.Any())
            {
                var maxNoteId = notes.Select(n => n.Id).Max();
                note.Id = ++maxNoteId;
            }
            else
            {
                note.Id = 1;
            }
        }

        public async Task<bool> RemoveNote(NoteEntryModel note)
        {
            var notes = await GetNotes();

            var noteToRemove = notes?.FirstOrDefault(n => n.Id == note.Id);

            if (noteToRemove == null)
            {
                return false;
            }

            notes.Remove(noteToRemove);

            return await SaveNotes(notes);
        }

        public async Task<List<NoteEntryModel>> GetNotes()
        {
            var fileExists = await _rootFolder.CheckExistsAsync(Constants.DataFileName);

            if (fileExists == ExistenceCheckResult.NotFound)
            {
                return new List<NoteEntryModel>();
            }

            var file = await _rootFolder.GetFileAsync(Constants.DataFileName);

            var fileData = await file.ReadAllTextAsync();

            var notes = JsonConvert.DeserializeObject<List<NoteEntryModel>>(fileData);

            file = null;
            fileData = null;

            return notes ?? new List<NoteEntryModel>();
        }

        private async Task<bool> SaveNotes(List<NoteEntryModel> notes)
        {
            try
            {
                var updatedFile =
                    await _rootFolder.CreateFileAsync(Constants.DataFileName, CreationCollisionOption.ReplaceExisting);

                await updatedFile.WriteAllTextAsync(JsonConvert.SerializeObject(notes));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public async Task<ObservableCollection<NoteEntryModel>> SetUp()
        {
            try
            {
                var result = await _rootFolder.CheckExistsAsync(Constants.DataFileName);

                if (result == ExistenceCheckResult.NotFound)
                {
                    await _rootFolder.CreateFileAsync(Constants.DataFileName, CreationCollisionOption.ReplaceExisting);
                }

                var file = await _rootFolder.GetFileAsync(Constants.DataFileName);

                var data = await file.ReadAllTextAsync();

                return data == null ? new ObservableCollection<NoteEntryModel>() :new ObservableCollection<NoteEntryModel>(JsonConvert.DeserializeObject<List<NoteEntryModel>>(data));

            }
            catch (Exception ex)
            {
                return new ObservableCollection<NoteEntryModel>();
            }
        }

        public async Task<string> NoteExists(NoteEntryModel note)
        {
            if (note.Id > 0)
            {
                return string.Empty;
            }

            var notes = await GetNotes();

            var noteExists = notes.Any(n => n.Title.Equals(note.Title, StringComparison.CurrentCultureIgnoreCase));

            return noteExists ? "Note with same name already exists" : string.Empty;
        }
    }
}
