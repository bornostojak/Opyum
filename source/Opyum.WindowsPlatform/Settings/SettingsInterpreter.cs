using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Opyum.WindowsPlatform.Settings;
using System.Threading;
using System.Collections;

namespace Opyum.WindowsPlatform.Settings
{
    public static class SettingsInterpreter
    {
        //public static XmlDocument SettingsXML { get; private set; }
        private static string JsonSettings { get; set; } = string.Empty;
        private static int _errorCount = 0;

        /// <summary>
        /// Load settings from the default location
        /// <para>Gets all the files in the Settings directory and loads them</para>
        /// </summary>
        public static SettingsContainer LoadSettings()
        {
            return Load(Directory.CreateDirectory(SettingsManager.GetSettingsDirectoryPath()).GetFiles().Select(x => x.FullName));
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

        internal static string GetJsonFormFile(string file)
        {
            //var containig the text of the file
            string fileText;
            //check if the file exists and is in the poper folder
            if (!File.Exists(file) || Path.GetDirectoryName(file) != SettingsManager.GetSettingsDirectoryPath())
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

        internal static string GetListText(IList obj)
        {
            string text = string.Empty;
            foreach (var item in obj)
            {
                text += $"{(text == string.Empty ? "" : ",\n")}{JsonConvert.SerializeObject(item)}";
            }
            return text;
        }

    }
}
