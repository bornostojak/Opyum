﻿using System;
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
    [OpyumSettingsPanelElement(typeof(ShortcutKeyBinding[]))]
    public partial class ShortcutPanelElement : UserControl, ISettingsPanel, IUndoRedoable
    {
        List<ListViewItem> Data { get => _data; set => _data = value; } 
        List<ListViewItem> _data = null;
        UndoRedoStack UndoRedo { get; set; } = new UndoRedoStack();
        public event SettingsChangedEventHandler SettingsChanged;

        public ShortcutPanelElement()
        {
            InitializeComponent();
            this.KeyDown += KeyPressResolve;
            this.listviewshortcuts.ItemSelectionChanged += ItemSelected;
            this.isGlobalCheckBox.CheckedChanged += globalChecked;
            this.isDisabledCheckBox.CheckedChanged += disabledChecked;
            UndoRedo.UndoRedoStackChanged += (a, b) => { SettingsChanged?.Invoke(a, new SettingsChangedEventArgs(UndoRedo.UndoCount)); };
            buttonSaveShortcut.Enabled = false;
            this.Show();
            //ShortcutSwapPair.SettingsChanged += (a, b) => { SettingsChanged?.Invoke(a, b); };



            //block escape close when on textBox
            this.textBoxShortcut.GotFocus += (a, b) => { SettingsEditor.BlockEscapeClose = true; };
            this.textBoxShortcut.LostFocus += (a, b) => { SettingsEditor.BlockEscapeClose = false; };
        }

        private void ItemSelected(object sender, EventArgs e)
        {
            if (listviewshortcuts.SelectedItems != null && listviewshortcuts.SelectedItems.Count > 0)
            {
                textBoxShortcut.Text = ShortcutResolver.GetShortcutString(((IShortcutKeyBinding)listviewshortcuts.SelectedItems[0].Tag).ShortcutKeys);
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
            checkForChange();
        }

        private void disabledChecked(object sender, EventArgs e)
        {
            checkForChange();
        }

        private void checkForChange()
        {
            if (listviewshortcuts.SelectedItems.Count > 0 && (
                ((IShortcutKeyBinding)listviewshortcuts.SelectedItems[0].Tag).IsDisabled != isDisabledCheckBox.Checked ||
                ((IShortcutKeyBinding)listviewshortcuts.SelectedItems[0].Tag).Global != isGlobalCheckBox.Checked ||
                (textBoxShortcut.Tag != null && !((IEnumerable<Keys>)textBoxShortcut.Tag).SequenceEqual(((IShortcutKeyBinding)listviewshortcuts.SelectedItems[0].Tag).ShortcutKeys))))
            {
                buttonSaveShortcut.Enabled = true;
                return;
            }
            buttonSaveShortcut.Enabled = false;
        }


        public object LoadElements()
        {
            if (listviewshortcuts.Items.Count > 0)
                listviewshortcuts.Items.Clear();
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
            e.Handled = true;
            var shortcuts = ShortcutResolver.GrabShortcut(e);
            if (shortcuts == null) return;
            textBoxShortcut.Clear();
            textBoxShortcut.Text = ShortcutResolver.GetShortcutString(shortcuts);
            textBoxShortcut.Text = textBoxShortcut.Text == "Back" ? "" : textBoxShortcut.Text;
            textBoxShortcut.Tag = shortcuts;
            checkForChange();
            textBoxAssigned.Text = SettingsEditor.Settings.NewSettings.Shortcuts.FirstOrDefault(x => x.ShortcutKeys.SequenceEqual(shortcuts))?.Action;
        }

        private void buttonClearShortcut_Click(object sender, EventArgs e)
        {
            if (listviewshortcuts.SelectedItems.Count > 0)
            {
                UndoRedo.Do<ListViewItem, IEnumerable<Keys>>(ChangeShortcut, listviewshortcuts.SelectedItems[0], new Keys[0], this);
                return;
            }
        }

        private void buttonSaveShortcut_Click(object sender, EventArgs e)
        {
            if (textBoxShortcut.Tag != null && listviewshortcuts.SelectedItems.Count > 0)
            {
                var shortcuts = (IEnumerable<Keys>)textBoxShortcut.Tag;
                var owner = Data.FirstOrDefault(x => ((IShortcutKeyBinding)x.Tag).ShortcutKeys.SequenceEqual(shortcuts));
                var recipient = listviewshortcuts.SelectedItems[0];

                if (owner != null)
                {
                    if (MessageBox.Show($"This string is already in use by \"{((IShortcutKeyBinding)owner.Tag).Action}\".\nDo you want to owerwrite it?\n\nWARNING: the old shortcut will be deleted!", "nWARNING: shortcut already in use!", MessageBoxButtons.YesNo, icon: MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return;
                    }

                    //requiest to execute the operation
                    UndoRedo.Do<ShortcutSwapPair, Keys[]>(ShortcutSwapPair.Swap, new ShortcutSwapPair(recipient, owner), new Keys[0]);
                    return;
                }
                UndoRedo.Do<ListViewItem, IEnumerable<Keys>>(ChangeShortcut, recipient, shortcuts, this);
            }
            buttonSaveShortcut.Enabled = false;
        }

        public IEnumerable<Keys> ChangeShortcut(ListViewItem item, IEnumerable<Keys> srt)
        {
            var o = (IShortcutKeyBinding)item.Tag;
            var old = o.ShortcutKeys.ToArray();
            o.UpdateShortcut(srt);
            item.SubItems[1].Text = ShortcutResolver.GetShortcutString(o.ShortcutKeys);
            //SettingsChanged?.Invoke(((IShortcutKeyBinding)item.Tag), new SettingsChangedEventArgs(o.ShortcutKeys.Sum(b => (int)b)));
            return old;
        }


        public void KeyPressResolve(object sender, KeyEventArgs e)
        {
            if (!textBoxShortcut.Focused)
            {
                if (e.KeyData == (Keys.Control | Keys.Z))
                {
                    UndoRedo.Undo();
                }
                if (e.KeyData == (Keys.Control | Keys.Shift | Keys.Z))
                {
                    UndoRedo.Redo();
                } 
            }
        }

        public void Undo()
        {
            UndoRedo?.Undo();
        }

        public void Redo()
        {
            UndoRedo?.Redo();
        }


        public class ShortcutSwapPair
        {
            public static event SettingsChangedEventHandler SettingsChanged;
            private ListViewItem NEW { get; set; }

            private ListViewItem OLD { get; set; }


            public ShortcutSwapPair(ListViewItem newVar, ListViewItem oldVar)
            {
                NEW = newVar;
                OLD = oldVar;
            }
            public static Keys[] Swap(ShortcutSwapPair pair, Keys[] keys)
            {
                var temp = ((IShortcutKeyBinding)pair.NEW.Tag).ShortcutKeys.ToArray();
                var oldsum = ((IShortcutKeyBinding)pair.NEW.Tag).ShortcutKeys.Sum(b => (int)b)+ ((IShortcutKeyBinding)pair.NEW.Tag).ShortcutKeys.Sum(b => (int)b);

                ((IShortcutKeyBinding)pair.NEW.Tag).UpdateShortcut(((IShortcutKeyBinding)pair.OLD.Tag).ShortcutKeys.ToArray());
                ((IShortcutKeyBinding)pair.OLD.Tag).UpdateShortcut(keys);
                var swap = pair.NEW;
                pair.NEW = pair.OLD;
                pair.OLD = swap;


                pair.NEW.SubItems[1].Text = ShortcutResolver.GetShortcutString(((IShortcutKeyBinding)pair.NEW.Tag).ShortcutKeys);
                pair.OLD.SubItems[1].Text = ShortcutResolver.GetShortcutString(((IShortcutKeyBinding)pair.OLD.Tag).ShortcutKeys);


                SettingsChanged?.Invoke(pair, new SettingsChangedEventArgs(oldsum));
                return temp;
            }
        }
    }
}
