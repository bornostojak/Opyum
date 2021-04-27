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

        static FileSystemWatcher settingsFSWatcher = new FileSystemWatcher()
        {
            Path = SettingsInterpreter.GetSettingsDirectoryPath().FullName,
            NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
            Filter = "*.json",
            EnableRaisingEvents = true
        };

        //Monitors wheather any importan file for the application has changed
        private void SettingsFileSystemWatcherSetup()
        {
            settingsFSWatcher.Changed += (a, e) =>
            {
                if (!SettingsInterpreter.SavingSettings)
                {
                    SettingsManager.UpdateSettingsFromFile(e.FullPath);
                }
            };
        }

    }
}
