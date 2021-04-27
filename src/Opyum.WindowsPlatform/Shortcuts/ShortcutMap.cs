using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Opyum.WindowsPlatform.Shortcuts
{
    public class ShortcutMap
    {
        public Keys Shortcut { get; set; } = 0;

        public List<ShortcutMap> Map { get; protected set; }

        public int Count => this.Map.Count;


    }
}
