using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.WindowsPlatform.Settings
{
    public class SettingsChangedEventArgs : EventArgs
    {
        public int UnsavedSettingsCount { get; set; } = 0;
        public SettingsChangedEventArgs()
        {

        }

        public SettingsChangedEventArgs(int changes) : this()
        {
            UnsavedSettingsCount = changes;
        }
    }

    public delegate void SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e);
}
