using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public static SettingsContainer GlobalSettings { get; set; }





        public static string GetSettingsDirectoryPath()
        {
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"..\Settings"));
        }

        public static void LoadSettings()
        {
            GlobalSettings = SettingsInterpreter.LoadSettings();

            //get all shortcuts from the entire program, remove their default shortcuts and add to shortcut list
            GlobalSettings?.Shortcuts.AddRange(ShortcutManager.GetShortcutsFromAssembliesInexecutingFolder());
        }


        public static void SaveSettings()
        {
            //SettingsManager.GlobalSettings?.Shortcuts = SettingsManager.GlobalSettings?.Shortcuts?.OrderBy(u => u.Command).ToList();
            SettingsManager.GlobalSettings?.Shortcuts?.Sort((x, y) => x.Action.CompareTo(y.Action));


            foreach (var item in SettingsManager.GlobalSettings.GetType().GetProperties())
            {
                if (item.Name == "Item")
                {
                    continue;
                }
                string text = string.Empty;
                var obj = item.GetValue(SettingsManager.GlobalSettings);
                if (obj is IList)
                {
                    text = $"[\n{SettingsInterpreter.GetListText((IList)obj).Replace("{", "\t{")}\n]";
                }
                else
                {
                    text = JsonConvert.SerializeObject(item.GetValue(SettingsManager.GlobalSettings), Formatting.Indented);
                }

                try
                {
                    File.WriteAllText($"{SettingsManager.GetSettingsDirectoryPath()}\\{item.Name}.json", text);
                }
                catch (Exception q)
                {
                    throw q;
                }
            }
        }

        public static void UpdateSettingsFromFile(string path)
        {
            //check if the file exists and is in the poper folder
            if (!File.Exists(path) || Path.GetDirectoryName(path) != GetSettingsDirectoryPath())
            {
                return;
            }

            //update the settings by reading the Json from the file
            UpdateSettings(JsonConvert.DeserializeObject<SettingsContainer>($"{{{SettingsInterpreter.GetJsonFormFile(path)}}}"));
        }

        public static void UpdateSettings(SettingsContainer settings)
        {
            //get all properties from the type of settings and make sure they are public and instantiated
            var props = new List<PropertyInfo>(settings.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public));
            
            //remo the "Item" property representing the SettingsContainer's indexer property <this[string property] {get; set;}>
            props.RemoveAll(t => t.Name == "Item");

            //update each non-null property in settings to GlobalSettings
            foreach (var prop in props)
            {
                if (settings[prop.Name] != null)
                {
                    //if the property exists, put it in the GlobalSettings
                    //then remove it from the settings obejct so it can be disposed
                    GlobalSettings[prop.Name] = settings[prop.Name];
                    settings[prop.Name] = null;
                }
            }
            //clear the garbage
            settings.Dispose();
        }
    }
}
