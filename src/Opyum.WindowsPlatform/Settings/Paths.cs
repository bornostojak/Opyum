using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Opyum.WindowsPlatform
{
    public static class Paths
    {
        private static string _settingsFilePathJSON = $"{SettingsDirectoryPath}\\Settings.json";
                
        public static string SettingsJSONFilePath
        {
            get { return _settingsFilePathJSON; }
            set { _settingsFilePathJSON = value; }
        }

        private static string _settingsDirectoryPath = $"{Path.GetDirectoryName(Path.GetDirectoryName((System.Reflection.Assembly.GetEntryAssembly().Location)))}\\Settings";

        public static string SettingsDirectoryPath
        {
            get { return _settingsDirectoryPath; }
            set { _settingsDirectoryPath = value; }
        }


        public static string BaseSettingsPath { get; private set; } = $"{SettingsDirectoryPath}\\Basesettings.json";

        public static string ShortcutJSONPath { get; private set; } = $"{SettingsDirectoryPath}\\Shortcuts.json";

    }
}
