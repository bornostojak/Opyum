using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Opyum.WindowsPlatform.Settings;
using Opyum.WindowsPlatform.Attributes;

namespace Opyum.WindowsPlatform
{
    public partial class MainWindow
    {
        private void OpenSettings_OnClick(object sender, EventArgs e)
        {
            OpenSettings();
        }

        [OpyumShortcutMethod("open_settings", Action = "Open settings", Description = "Opens the settings.")]
        public void OpenSettings(string[] args = null)
        {
            Settings.SettingsEditor.OpenSettings();
        }
    }
}
