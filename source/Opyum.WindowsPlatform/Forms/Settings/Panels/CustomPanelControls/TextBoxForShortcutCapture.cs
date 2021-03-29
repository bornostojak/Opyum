using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Opyum.WindowsPlatform.Settings.Panels.CustomPanelControls
{
    public class TextBoxForShortcutCapture : TextBox
    {
        protected override bool ProcessCmdKey(ref Message m, Keys keyData)
        {
            return false;
        }
    }
}
