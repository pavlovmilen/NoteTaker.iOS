using NoteTaker.Core.Models;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NoteTaker.Core.ViewModels
{
    public class NotesViewModel
    {
        private IFolder _rootFolder;

        public ObservableCollection<NoteEntryModel> Notes { get; set; } = new ObservableCollection<NoteEntryModel>();

        public NotesViewModel()
        {
        }

        public async Task SetUp()
        {
            _rootFolder = FileSystem.Current.LocalStorage;
            var result = await _rootFolder.CheckExistsAsync(Constants.DataFileName);

            if (result == ExistenceCheckResult.NotFound)
            {
                await _rootFolder.CreateFileAsync(Constants.DataFileName, CreationCollisionOption.ReplaceExisting);
            }

            var file = await _rootFolder.GetFileAsync(Constants.DataFileName);

            var data = await file.ReadAllTextAsync();

            Notes = new ObservableCollection<NoteEntryModel>(JsonConvert.DeserializeObject<List<NoteEntryModel>>(data));
        }

        public async Task<bool> AddEntry(NoteEntryModel entry)
        {

            var fileExists = await _rootFolder.CheckExistsAsync(Constants.DataFileName);

            if (fileExists == ExistenceCheckResult.NotFound)
            {
                return false;
            }

            var file = await _rootFolder.GetFileAsync(Constants.DataFileName);

            var fileData = await file.ReadAllTextAsync();

            var notes = JsonConvert.DeserializeObject<List<NoteEntryModel>>(fileData);


        }

        public async Task<bool> RemoveEntry(NoteEntryModel entry)
        {
            var fileExists = await _rootFolder.CheckExistsAsync(Constants.DataFileName);

            if (fileExists == ExistenceCheckResult.NotFound)
            {
                return false;
            }

            var file = await _rootFolder.GetFileAsync(Constants.DataFileName);

            var fileData = await file.ReadAllTextAsync();

            var notes = JsonConvert.DeserializeObject<List<NoteEntryModel>>(fileData);

            var noteToRemove = notes?.FirstOrDefault(n => n.Id == entry.Id);

            if (noteToRemove == null)
            {
                return false;
            }

            notes.Remove(noteToRemove);

            var updatedFile =
                await _rootFolder.CreateFileAsync(Constants.DataFileName, CreationCollisionOption.ReplaceExisting);

            await updatedFile.WriteAllTextAsync(JsonConvert.SerializeObject(notes));

            return true;
        }
    }
}
