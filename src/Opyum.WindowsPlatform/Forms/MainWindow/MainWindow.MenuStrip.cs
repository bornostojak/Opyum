using System.Windows.Forms;
using Opyum.WindowsPlatform.Settings;
using Opyum.WindowsPlatform.Attributes;
using System;
using System.Linq;

namespace Opyum.WindowsPlatform
{
    public partial class MainWindow
    {
        private void MenuStripSetup()
        {

        }

        [OpyumShortcutMethod("hide_menu_strip", Description = "Show or hide the menu.", Action = "Hide menu")]
        public void MenuStripOnCall(string[] args = null)//object sender, EventArgs e)
        {
            //if (MenuStrip.Height > 70 && MenuStrip.Visible)
            //{
            //    MenuStrip.Visible = false;
            //    return;
            //}
            //MenuStrip.Visible = true;
            //MenuStrip.Height = MenuStrip.Height < 20 ? MenuStrip.Height * 4 : 18;

            //foreach (ToolStripMenuItem item in MenuStrip.Items) item.Width = MenuStrip.Height < 20 ? (int)(item.Width / 4) : (int)(item.Width * 4);

            ////ResizeMenuStripItems(MenuStrip.Items, MenuStrip.Height < 20 ? 1 / 4 : 4);

            //MenuStrip.Show();
            MenuStrip.Visible = !MenuStrip.Visible;
        }

        private void ResizeMenuStripItems(System.Windows.Forms.ToolStripItemCollection ctrl, double multiplyer)
        {
            if (ctrl.Count > 0)
            {
                foreach (ToolStripMenuItem control in ctrl)
                {
                    control.AutoSize = false;
                    if (control.DropDownItems.Count > 0)
                    {
                        ResizeMenuStripItems(control.DropDownItems, multiplyer);
                    }
                    control.Height = (int)(control.Height * multiplyer);
                }
            }
        }


        /// <summary>
        /// Starts updating the shortcuta written in the menustrip bar
        /// </summary>
        private void UpdateMenuStrip(object sender, EventArgs e)
        {
            UpdateMenuStrip();
        }


        /// <summary>
        /// Starts updating the shortcuta written in the menustrip bar
        /// </summary>
        private void UpdateMenuStrip()
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
