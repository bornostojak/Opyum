using System;
using Opyum.WindowsPlatform.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Opyum.WindowsPlatform
{
    public partial class MainWindow
    {

        FileSystemWatcher watcher = new FileSystemWatcher();

        //Monitors wheather any importan file for the application has changed
        private void FileSystemWatcherSetup()
        {
            watcher.Path = Paths.SettingsDirectoryPath;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.json";
            watcher.EnableRaisingEvents = true;
            watcher.Changed += (a, b) =>
            {
                SettingsManager.UpdateSettingsFromFile(b.FullPath);
                this.Invoke(new Action(MenuStrip_Shortcut_Update));
            };
        }
    }
}
