using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Opyum.WindowsPlatform.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OpyumShortcutMethodAttribute : Attribute
    {
        public static List<OpyumShortcutMethodAttribute> All { get; protected set; } = new List<OpyumShortcutMethodAttribute>();
        public string Command { get; protected set; }
        public string Description { get; set; }
        public string Action { get; set; }

        public OpyumShortcutMethodAttribute(string command)
        {
            Command = command;
            All.Add(this);
        }

        public OpyumShortcutMethodAttribute(string command, List<Keys> shortcut)
        {
            Command = command;
            DefaultShortcut = shortcut;
            All.Add(this);
        }

        public OpyumShortcutMethodAttribute(string command, Keys[] shortcut)
        {
            Command = command;
            DefaultShortcut = shortcut.ToList();
            All.Add(this);
        }

        public List<Keys> DefaultShortcut { get; protected set; } = new List<Keys>();
    }
}
