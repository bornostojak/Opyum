using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Opyum.WindowsPlatform.Settings;
using System.Threading;
using System.Collections;
using Opyum.WindowsPlatform.Shortcuts;
using System.Reflection;

namespace Opyum.WindowsPlatform.Settings
{
    public static class SettingsInterpreter
    {
        public static bool SavingSettings { get; set; } = false;
        //public static XmlDocument SettingsXML { get; private set; }
        private static string JsonSettings { get; set; } = string.Empty;
        private static int _errorCount = 0;

        /// <summary>
        /// Load settings from the default location
        /// <para>Gets all the files in the Settings directory and loads them</para>
        /// </summary>
        public static SettingsContainer LoadSettings()
        {
            return Load(SettingsInterpreter.GetSettingsDirectoryPath().GetFiles().Select(x => x.FullName));
        }

        /// <summary>
        /// Load settings from multiple files
        /// <para>The files must be written in JSON notation</para>
        /// </summary>
        public static SettingsContainer Load(IEnumerable<string> locations)
        {
            locations = locations.Where(u => u.EndsWith(".json"));
            JsonSettings = string.Empty;

            //JsonSettings = "{";
            int filecount = 0;

            foreach (var file in locations)
            {
                var fileText = GetJsonFormFile(file);
                if (fileText != string.Empty)
                {
                    JsonSettings += $"{(filecount++ > 0 ? "," : "")}{fileText}";
                }
            }
            //converge all the key-value json pairs one object
            JsonSettings = $"{{{JsonSettings}}}";
            var gg = JsonConvert.DeserializeObject<SettingsContainer>(JsonSettings);

            return gg;
        }

        public static IEnumerable<ShortcutKeyBinding> GetShortcutsFromFile()
        {
            return LoadFromFile<ShortcutKeyBinding[]>($"{SettingsInterpreter.GetSettingsDirectoryPath().FullName}\\Shortcuts.json");
        }

        /// <summary>
        /// Load settings from specific file
        /// <para>The files must be written in JSON notation</para>
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        public static T LoadFromFile<T>(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("A settings file was not found!", path);
            }

            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        /// <param name="settings"></param>
        public static void SaveSettings(SettingsContainer settings)
        {
            SavingSettings = true;
            foreach (var item in settings.GetType().GetProperties())
            {
                if (item.Name == "Item")
                {
                    continue;
                }
                string text = string.Empty;
                var obj = item.GetValue(settings);
                if (obj is IEnumerable)
                {
                    text = $"[\n{SettingsInterpreter.GetObjectText(obj).Replace("{", "\t{")}\n]";
                }
                else
                {
                    text = JsonConvert.SerializeObject(item.GetValue(settings), Formatting.Indented);
                }

                try
                {
                    File.WriteAllText($"{GetSettingsDirectoryPath().FullName}\\{item.Name}.json", text);
                }
                catch (Exception q)
                {
                    throw q;
                }
            }
            SavingSettings = false;
        }


        internal static string GetJsonFormFile(string file)
        {
            //var containig the text of the file
            string fileText;
            //check if the file exists and is in the poper folder
            if (!File.Exists(file) || Path.GetDirectoryName(file) != SettingsInterpreter.GetSettingsDirectoryPath().FullName)
            {
                return string.Empty;
            }

            try
            {
                fileText = File.ReadAllText(file);
                JContainer.Parse(fileText);

            }
            catch //(IOException)
            {
                //if the file is stalled by another program, wait 2s 
                if (_errorCount++ < 40)
                {
                    Thread.Sleep(50);
                    _errorCount++;
                    return GetJsonFormFile(file);
                }
                else
                    //after 2 seconds ignore the file and send empty string
                    return string.Empty;

                //needs to send a error message to notify that the josn string is not working and from which file.

            }
            _errorCount = 0;
            //concat the file name as the parameter name and the file contents as the parameter value
            return $"\"{Path.GetFileNameWithoutExtension(file)}\":{fileText}";
        }

        

        /// <summary>
        /// Serialize an object to a JSON string.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static string GetObjectText(object obj)
        {
            if (obj is IEnumerable)
            {
                return GetObjectText((IEnumerable)obj);
            }
            return JsonConvert.SerializeObject(obj);
        }

        private static string GetObjectText(IEnumerable obj)
        {
            string text = string.Empty;
            foreach (var item in obj)
            {
                text += $"{(text == string.Empty ? "" : ",\n")}{GetObjectText(item)}";
            }
            return text;
        }



        public static DirectoryInfo GetSettingsDirectoryPath()
        {
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"..\Settings"));
            return Directory.CreateDirectory(path);
        }

        public static string GetSettingsPathFor(SettingsComponent component)
        {
            switch (component)
            {
                case SettingsComponent.Shortcuts:
                    return Path.Combine(GetSettingsDirectoryPath().FullName, "Shortcuts.json");
                case SettingsComponent.Colors:
                    return Path.Combine(GetSettingsDirectoryPath().FullName, "Colors.json");
                case SettingsComponent.Plugins:
                    return Path.Combine(GetSettingsDirectoryPath().FullName, "Plugins.json");
                default:
                    return string.Empty;
            }
        }
    }
}
