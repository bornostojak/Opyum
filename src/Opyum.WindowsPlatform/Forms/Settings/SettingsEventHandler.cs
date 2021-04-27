using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.WindowsPlatform.Settings
{
    public class SettingsChangedEventArgs : EventArgs
    {
        public int OldState { get; set; } = 0;
        public SettingsChangedEventArgs()
        {

        }

        public SettingsChangedEventArgs(int changes) : this()
        {
            OldState = changes;
        }
    }

    public delegate void SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e);
}
