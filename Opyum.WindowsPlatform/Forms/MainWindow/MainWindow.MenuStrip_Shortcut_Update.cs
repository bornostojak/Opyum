using Opyum.WindowsPlatform.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Opyum.WindowsPlatform
{
    public partial class MainWindow : Form
    {
        /// <summary>
        /// Starts updating the shortcuta written in the menustrip bar
        /// </summary>
        private void MenuStrip_Shortcut_Update()
        {

            UpdateToolStripItemCollection(MenuStrip.Items);
            return;
        }

        /// <summary>
        /// Updateds the shortcuts in the <see cref="MainWindow.MenuStrip"/> based on the <see cref="SettingsManager.GlobalSettings"/>
        /// </summary>
        /// <param name="coll"></param>
        private void UpdateToolStripItemCollection(ToolStripItemCollection coll)
        {
            foreach (var itemz in coll)
            {
                if (!(itemz is ToolStripMenuItem)) continue;
                var item = (ToolStripMenuItem)itemz;
                try
                {
                    if (item != null && item.Tag != null)
                    {
                        item.ShortcutKeyDisplayString = string.Join(", ", SettingsManager.GlobalSettings.Shortcuts.Where(x => item.Tag == null ? false : x.Command == (string)item.Tag && x.Function != null)?.FirstOrDefault()?.Shortcut);

                    }
                }
                catch (ArgumentNullException)
                {

                }
                
                if (item.HasDropDownItems)
                {
                    UpdateToolStripItemCollection(item.DropDownItems);
                }
            }
        }
    }
}