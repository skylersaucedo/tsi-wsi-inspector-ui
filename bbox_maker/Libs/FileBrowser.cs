using bbox_maker.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bbox_maker
{
    internal class FileBrowser
    {
        static FileBrowser _instance;
        public static FileBrowser Instance
        {
            get
            {
                if (_instance == null) { _instance = new FileBrowser(); }
                return _instance;
            }
        }

        private readonly Dictionary<string, string> RecentDirectoriesByTitle = new Dictionary<string, string>();
        private void SetRecentDirectory(string title, string path)
        {
            string directory = Path.GetDirectoryName(path);
            if (RecentDirectoriesByTitle.ContainsKey(title))
            {
                RecentDirectoriesByTitle.Remove(title);
            }
            RecentDirectoriesByTitle.Add(title, directory);

            if (Settings.Default.LastDirectoryPath != directory)
            {
                Settings.Default.LastDirectoryPath = directory;
            }
        }

        public string GetRecentDirectory(string title)
        {
            string directory = null;
            if (RecentDirectoriesByTitle.ContainsKey(title))
            {
                directory = RecentDirectoriesByTitle[title];
            }
            else
            {
                directory = String.IsNullOrEmpty(Settings.Default.LastDirectoryPath) ?
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop) :
                    Settings.Default.LastDirectoryPath;
            }
            return directory;
        }

        public string OpenDirectory(string title = "Select folder")
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = title;
                dialog.SelectedPath = GetRecentDirectory(title);
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    SetRecentDirectory(title, dialog.SelectedPath);
                    return dialog.SelectedPath;
                }
            }
            return null;
        }

        public string OpenFilename(string filter, string title = "Select file")
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = GetRecentDirectory(title);
            fileDialog.CheckFileExists = true;
            fileDialog.Filter = filter;     // "CSV Files(*.csv)|*.csv"
            fileDialog.Title = title;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                SetRecentDirectory(title, fileDialog.FileName);
                return fileDialog.FileName;
            }
            return null;
        }

        public string SaveFilename(string defaultExt, string filter, string title = "Save File")
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.AddExtension = true;
            fileDialog.CheckPathExists = true;
            fileDialog.ValidateNames = true;
            fileDialog.OverwritePrompt = true;
            fileDialog.DefaultExt = defaultExt;     // ".csv";
            fileDialog.Filter = filter;     // "CSV Files(*.csv)|*.csv";
            fileDialog.InitialDirectory = GetRecentDirectory(title);
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                SetRecentDirectory(title, fileDialog.FileName);
                return fileDialog.FileName;
            }
            return null;
        }
    }
}
