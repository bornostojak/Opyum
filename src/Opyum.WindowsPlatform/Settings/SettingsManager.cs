using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opyum.WindowsPlatform.Shortcuts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Opyum.WindowsPlatform.Settings
{
    public static class SettingsManager 
    {
        public static SettingsContainer GlobalSettings { get; } = new SettingsContainer();
        //public static SettingsContainer GlobalSettings { get => _settings; set => _settings.Update(value); }
        //static SettingsContainer _settings = new SettingsContainer();


        /// <summary>
        /// Pull shortcuts from default settings path or custom settings file
        /// <para>The files must be written in JSON notation</para>
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        public static ShortcutKeyBinding[] PullShortcutsFromFile(string path = null)
        {
            //TODO: make sure the settings are valid
            //TODO: make sure the path exists

            var shortcuts = SettingsInterpreter.LoadFromFile<ShortcutKeyBinding[]>(path == null || path == string.Empty ? SettingsInterpreter.GetSettingsPathFor(SettingsComponent.Shortcuts) : path);
            return shortcuts;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void LoadSettings()
        {
            //pull the shortcuts from file, then from the assembly, and user the shortcuts from the file first
            //the update shortcuts method will deal with shortcut function allocation
            var shortcuts = PullShortcutsFromFile().Concat(ShortcutManager.GetShortcutsFromAssembliesInExecutingAssemblies()).GroupBy(x => x.Command).Select(x => x.First());
            GlobalSettings.UpdateShortcuts(shortcuts);

            //get all shortcuts from the entire program, remove their default shortcuts and add to shortcut list
        }



        public static void UpdateSettingsFromFile(string path)
        {
            //check if the file exists and is in the poper folder
            if (!File.Exists(path) || Path.GetDirectoryName(path) != SettingsInterpreter.GetSettingsDirectoryPath().FullName)
            {
                return;
            }

            //update the settings by reading the Json from the file
            GlobalSettings.Update(JsonConvert.DeserializeObject<SettingsContainer>($"{{{SettingsInterpreter.GetJsonFormFile(path)}}}"));
        }
        public static async void UpdateSettingsFromFileAsync(string path)
        {
            //check if the file exists and is in the poper folder
            if (!File.Exists(path) || Path.GetDirectoryName(path) != SettingsInterpreter.GetSettingsDirectoryPath().FullName)
            {
                return;
            }

            //update the settings by reading the Json from the file
            await GlobalSettings.UpdateAsync(JsonConvert.DeserializeObject<SettingsContainer>($"{{{SettingsInterpreter.GetJsonFormFile(path)}}}"));
        }

    }
}
