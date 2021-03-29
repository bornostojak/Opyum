using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Opyum.Structures.Attributes;
using Opyum.WindowsPlatform.Settings;
using System.Reflection;
using Opyum.WindowsPlatform.Attributes;
using Opyum.WindowsPlatform.Shortcuts;
using System.IO;
using static System.Windows.Forms.ListView;
using System.Collections;
using Opyum.Structures.Global;

namespace Opyum.WindowsPlatform.Forms.Settings
{
    [OpyumSettingsPanelElement(typeof(List<ShortcutKeyBinding>))]
    public partial class ShortcutPanelElement : UserControl, ISettingsPanel, IUndoRedoable
    {
        List<ListViewItem> Data { get; set; } = null;
        UndoRedoStack UnredoStack { get; set; } = new UndoRedoStack();
        

        public ShortcutPanelElement()
        {
            InitializeComponent();
            this.KeyDown += KeyPressResolve;
            this.listviewshortcuts.ItemSelectionChanged += ItemSelected;
            this.isGlobalCheckBox.CheckedChanged += globalChecked;
            this.isDisabledCheckBox.CheckedChanged += disabledChecked;
            this.Show();
            
        }

        private void ItemSelected(object sender, EventArgs e)
        {
            if (listviewshortcuts.SelectedItems != null && listviewshortcuts.SelectedItems.Count > 0)
            {
                textBoxShortcut.Text = string.Join(", ", ((IShortcutKeyBinding)listviewshortcuts.SelectedItems[0].Tag).Shortcut);
                textBoxAssigned.Text = ((IShortcutKeyBinding)listviewshortcuts.SelectedItems[0].Tag).Action;
                isGlobalCheckBox.Enabled = true;
                isGlobalCheckBox.Checked = ((IShortcutKeyBinding)listviewshortcuts.SelectedItems[0].Tag).Global;
                isDisabledCheckBox.Enabled = true;
                isDisabledCheckBox.Checked = ((IShortcutKeyBinding)listviewshortcuts.SelectedItems[0].Tag).IsDisabled;
            }
            else
            {
                isGlobalCheckBox.Checked = false;
                isDisabledCheckBox.Checked = false;
                isGlobalCheckBox.Enabled = false;
                isDisabledCheckBox.Enabled = false;
                textBoxShortcut.Text = string.Empty;
            }
        }

        private void globalChecked(object sender, EventArgs e)
        {
            if (listviewshortcuts.SelectedItems.Count > 0)
            {
                //TODO
            }
        }

        private void disabledChecked(object sender, EventArgs e)
        {
            if (listviewshortcuts.SelectedItems.Count > 0)
            {
                //TODO
            }
        }


        public object LoadElements()
        {
            var p = SettingsEditor.Settings?.NewSettings?.Shortcuts;
            Data = p?.Select(g => GenerateItem(g)).ToList();
            listviewshortcuts.Items.AddRange(Data.ToArray());
            return this;

        }


        protected ListViewItem GenerateItem(IShortcutKeyBinding shortcut)
        {
            return new ListViewItem(new[] { shortcut.Action, string.Join(", ", shortcut.Shortcut.ToArray()), shortcut.Global ? "Yes" : "No", shortcut.IsDisabled ? "Yes" : "No", shortcut.Description }) { Tag = shortcut };
        }

        protected ListViewItem GenerateItem(ShortcutKeyBinding shortcut)
        {
            return GenerateItem((IShortcutKeyBinding)shortcut);
        }

        protected ListViewItem GenerateItem(OpyumShortcutMethodAttribute shortcut)
        {
            var keybinding = SettingsEditor.Settings.NewSettings.Shortcuts.Where(d => d.Command == shortcut.Command)?.FirstOrDefault();
            if (keybinding != null)
            {
                return GenerateItem(keybinding);
            }
            else
            {
                return new ListViewItem(new[] { shortcut.Action, "", "", "", shortcut.Description });
            }
            
        }

        //updates list viwe when something is typed in the search text box
        private void textBoxSearch_TextChange(object sender, EventArgs e)
        {
            if (textBoxSearch.Text != string.Empty && textBoxSearch != null)
            {
                listviewshortcuts.Items.Clear();
                listviewshortcuts.Items.AddRange(Data.Where(a => textBoxSearch?.Text?.ToLower()?.Split(' ')?.Select(b => (bool)a.SubItems[0].Text.ToLower()?.Contains(b) && b != string.Empty)?.Where(b => (bool)b)?.FirstOrDefault() == true ? true : false)?.ToArray());//listViewshortcuts.Items.AddRange((new List<ListViewItem>(lst))?.Where(a => textBoxSearch?.Text?.ToLower()?.Split(' ')?.Select(b => (bool)a.SubItems[0].Text.ToLower()?.Contains(b) && b != string.Empty)?.Where(b => (bool)b)?.FirstOrDefault() == true ? true : false)?.ToArray());
            }
        }

        //clear the search
        private void buttonClearSearch_Click(object sender, EventArgs e)
        {
            listviewshortcuts?.Items.Clear();
            textBoxSearch.Text = string.Empty;
            listviewshortcuts.Items.AddRange(Data.ToArray());
        }

        /// <summary>
        /// Get the shortcut when pressed on the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getShortcut(object sender, KeyEventArgs e)
        {
            var shortcuts = ShortcutResolver.GrabShortcut(e);
            if (shortcuts == null) return;
            textBoxShortcut.Clear();
            textBoxShortcut.Text = ShortcutResolver.GetShortcutString(shortcuts);
            textBoxShortcut.Text = textBoxShortcut.Text == "Back" ? "" : textBoxShortcut.Text;
            textBoxAssigned.Text = SettingsManager.GlobalSettings.Shortcuts.FirstOrDefault(x => x.ShortcutKeys.SequenceEqual(shortcuts))?.Action;
        }


        private void buttonSaveShortcut_Click(object sender, EventArgs e)
        {
            
            if (listviewshortcuts.SelectedItems.Count == 1 && textBoxShortcut.Text != string.Empty)
            {
                //check if the shortcut is already in use
                var existing = Data.Where(a => a.SubItems[1].Text == textBoxShortcut.Text);
                if (existing.Count() > 0 && existing.FirstOrDefault() != listviewshortcuts.SelectedItems[0])
                {
                    //asks if the olds shortcut should be deleted
                    if (MessageBox.Show($"This string is already in use by \"{existing?.FirstOrDefault()?.SubItems[0].Text}\".\nDo you want to owerwrite it?\n\nWARNING: the old shortcut will be deleted!", "nWARNING: shortcut already in use!", MessageBoxButtons.YesNo, icon: MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return;
                    }
                    //if the shortcut should be replaced
                    else
                    {
                        //create a list that will contain the operations that need to be executed
                        List<UndoRedoMethodCapsule> lst = new List<UndoRedoMethodCapsule>();
                        //empty the old shortcut
                        var cone = ((IShortcutKeyBinding)existing?.FirstOrDefault().Tag).Clone();
                        (cone.Global, cone.IsDisabled, cone.Shortcut) = (isGlobalCheckBox.Checked, isDisabledCheckBox.Checked, new List<string>());
                        lst.Add(new UndoRedoMethodCapsule(ChangeShortcut, existing?.FirstOrDefault(), cone, this));

                        //create the new shortcut
                        cone = ((IShortcutKeyBinding)listviewshortcuts.SelectedItems[0].Tag).Clone();
                        (cone.Global, cone.IsDisabled, cone.Shortcut) = (isGlobalCheckBox.Checked, isDisabledCheckBox.Checked, new List<string>(textBoxShortcut.Text.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)));
                        lst.Add(new UndoRedoMethodCapsule(ChangeShortcut, listviewshortcuts.SelectedItems[0], cone, this));

                        //requiest to execute the operation
                        UnredoStack.DoMany(lst);
                        return;
                    }
                }
                //if the new shortcut is not already in use
                var clone = ((ShortcutKeyBinding)listviewshortcuts.SelectedItems[0].Tag).Clone();
                clone.Shortcut = new List<string>(textBoxShortcut.Text.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries));
                (clone.Global, clone.IsDisabled) = (isGlobalCheckBox.Checked, isDisabledCheckBox.Checked);

                //requiest to execute the operation
                UnredoStack.Do(ChangeShortcut, listviewshortcuts.SelectedItems[0],  clone, this);
            }
        }

        public object ChangeShortcut(object o, object srt)
        {
            if (o is ListViewItem && srt is IShortcutKeyBinding)
            {
                //grab the old shortcut
                var old = ((IShortcutKeyBinding)((ListViewItem)o).Tag).Clone();
                //update the shortcut info
                ((IShortcutKeyBinding)((ListViewItem)o).Tag).UpdateDataFromKeybinding((IShortcutKeyBinding)srt);

                //update all the subitems in the selected item ListView
                for (int i = 0;  i < ((ListViewItem)o).SubItems.Count;  i++)
                {
                    ((ListViewItem)o).SubItems[i] = GenerateItem((IShortcutKeyBinding)((ListViewItem)o).Tag).SubItems[i];
                }
                return old; 
            }
            return new List<string>();
        }

        public void KeyPressResolve(object sender, KeyEventArgs e)
        {
            if (!textBoxShortcut.Focused)
            {
                if (e.KeyData == (Keys.Control | Keys.Z))
                {
                    UnredoStack.Undo();
                }
                if (e.KeyData == (Keys.Control | Keys.Shift | Keys.Z))
                {
                    UnredoStack.Redo();
                } 
            }
        }

        public void Undo()
        {
            UnredoStack?.Undo();
        }

        public void Redo()
        {
            UnredoStack?.Redo();
        }
    }
}
