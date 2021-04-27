using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Opyum.WindowsPlatform.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
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

        public OpyumShortcutMethodAttribute(string command, string description = "", string action = "", Keys[] shortcut = null)
        {
            Command = command;
            Description = description;
            Action = action;
            if (shortcut != null)
            {
                DefaultShortcut = shortcut; 
            }
            All.Add(this);
        }

        public Keys[] DefaultShortcut { get; set; } = new Keys[0];
    }
}
