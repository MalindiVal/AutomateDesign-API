using Microsoft.Win32;
using Service.Interfaces;

namespace IHM.Services
{
    /// <summary>
    /// Implémentation WPF du service de dialogue de fichiers.
    /// </summary>
    public class WpfFileDialogService : IFileDialogService
    {
        /// <inheritdoc/>
        public string? OpenFile(string filter, string defaultExt)
        {
            var dialog = new OpenFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        /// <inheritdoc/>
        public string? OpenFolder()
        {
            var dialog = new OpenFolderDialog
            {
                Multiselect = false,
            };

            return dialog.ShowDialog() == true ? dialog.FolderName
                : null;
        }


        /// <inheritdoc/>
        public string? SaveFile(string filter, string defaultExt, string defaultName)
        {
            var dialog = new SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt,
                FileName = defaultName,
                AddExtension = true,
                OverwritePrompt = true
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}