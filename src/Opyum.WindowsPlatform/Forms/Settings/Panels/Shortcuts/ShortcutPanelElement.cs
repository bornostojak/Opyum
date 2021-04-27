using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Opyum.Structures.Attributes;
using Opyum.WindowsPlatform.Settings;
using Opyum.WindowsPlatform.Attributes;
using Opyum.WindowsPlatform.Shortcuts;
using Opyum.Structures.Global;

namespace Opyum.WindowsPlatform.Forms.Settings
{
    [OpyumSettingsPanelElement(typeof(ShortcutKeyBinding[]))]
    public partial class ShortcutPanelElement : UserControl, ISettingsPanel, IUndoRedoable
    {
        List<ListViewItem> Data { get; set; }
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
            buttonUpdateShortcut.Enabled = false;
            this.Show();

        }


        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (textBoxShortcut.Focused && ((~Keys.Shift & keyData) == Keys.Tab || keyData == Keys.Escape))
            {
                getShortcut(this, new KeyEventArgs(keyData));
                return true;
            }
            var res = base.ProcessDialogKey(keyData);
            return res;
        }

        private void ItemSelected(object sender, EventArgs e)
        {
            if (listviewshortcuts.SelectedItems != null && listviewshortcuts.SelectedItems.Count > 0)
            {
                var tag = ((IShortcutKeyBinding)listviewshortcuts.SelectedItems[0].Tag);
                textBoxShortcut.Text = ShortcutResolver.GetShortcutString(((IShortcutKeyBinding)listviewshortcuts.SelectedItems[0].Tag).ShortcutKeys);
                textBoxAssigned.Text = tag.Action;
                isGlobalCheckBox.Enabled = true;
                isGlobalCheckBox.Checked = tag.Global;
                isDisabledCheckBox.Enabled = true;
                isDisabledCheckBox.Checked = tag.IsDisabled;
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
                buttonUpdateShortcut.Enabled = true;
                return;
            }
            buttonUpdateShortcut.Enabled = false;
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
            //textBoxShortcut.Text = textBoxShortcut.Text == "Back" ? "" : textBoxShortcut.Text;
            textBoxShortcut.Tag = shortcuts;
            checkForChange();
            textBoxAssigned.Text = SettingsEditor.Settings.NewSettings.Shortcuts.FirstOrDefault(x => x.ShortcutKeys.SequenceEqual(shortcuts))?.Action;
        }

        private void buttonClearShortcut_Click(object sender, EventArgs e)
        {
            if (listviewshortcuts.SelectedItems.Count > 0)
            {
                var sel = ((IShortcutKeyBinding)listviewshortcuts.SelectedItems[0].Tag);
                UndoRedo.Do<ListViewItem, ShortcutState>(ChangeShortcut, listviewshortcuts.SelectedItems[0], new ShortcutState(new Keys[0], sel.Global, sel.IsDisabled), this);
                return;
            }
        }

        private void buttonUpdateShortcut_Click(object sender, EventArgs e)
        {
            if (listviewshortcuts.SelectedItems.Count > 0)
            {
                var recipient = listviewshortcuts.SelectedItems[0];
                IEnumerable<Keys> shortcuts = (IEnumerable<Keys>)textBoxShortcut.Tag == null ? ((IShortcutKeyBinding)recipient.Tag).ShortcutKeys.ToArray() : (IEnumerable<Keys>)textBoxShortcut.Tag;
                var owner = Data.FirstOrDefault(x => ((IShortcutKeyBinding)x.Tag).ShortcutKeys.SequenceEqual(shortcuts));

                if (owner != null && owner != recipient)
                {
                    if (MessageBox.Show($"This string is already in use by \"{((IShortcutKeyBinding)owner.Tag).Action}\".\nDo you want to owerwrite it?\n\nWARNING: the old shortcut will be deleted!", "nWARNING: shortcut already in use!", MessageBoxButtons.YesNo, icon: MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return;
                    }

                    //requiest to execute the operation
                    UndoRedo.Do<ShortcutSwapPair, ShortcutState>(ShortcutSwapPair.Swap, new ShortcutSwapPair(recipient, owner), new ShortcutState(new Keys[0], isGlobalCheckBox.Checked, isDisabledCheckBox.Checked));
                    return;
                }
                UndoRedo.Do<ListViewItem, ShortcutState>(ChangeShortcut, recipient, new ShortcutState(shortcuts, isGlobalCheckBox.Checked, isDisabledCheckBox.Checked), this);
            }
            buttonUpdateShortcut.Enabled = false;
        }

        public ShortcutState ChangeShortcut(ListViewItem item, ShortcutState srt)
        {
            var o = (IShortcutKeyBinding)item.Tag;
            var old = new ShortcutState(o.ShortcutKeys.ToArray(), o.Global, o.IsDisabled);
            o.UpdateShortcut(srt.Shortcut);
            o.Global = srt.Global;
            o.IsDisabled = srt.Disabled;
            UpdateListViewItem(item);
            //item.SubItems[1].Text = ShortcutResolver.GetShortcutString(o.ShortcutKeys);
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

        private static void UpdateListViewItem(ListViewItem item)
        {
            var skb = (IShortcutKeyBinding)item.Tag;
            item.SubItems[1].Text = string.Join(", ", skb.Shortcut);
            item.SubItems[2].Text = skb.Global ? "Yes" : "No";
            item.SubItems[3].Text = skb.IsDisabled ? "Yes" : "No";
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
            public static ShortcutState Swap(ShortcutSwapPair pair, ShortcutState exchange)
            {
                var temp = new ShortcutState(((IShortcutKeyBinding)pair.NEW.Tag).ShortcutKeys.ToArray(), ((IShortcutKeyBinding)pair.NEW.Tag).Global, ((IShortcutKeyBinding)pair.NEW.Tag).IsDisabled);
                var oldsum = ((IShortcutKeyBinding)pair.NEW.Tag).ShortcutKeys.Sum(b => (int)b)+ ((IShortcutKeyBinding)pair.NEW.Tag).ShortcutKeys.Sum(b => (int)b);

                ((IShortcutKeyBinding)pair.NEW.Tag).UpdateShortcut(((IShortcutKeyBinding)pair.OLD.Tag).ShortcutKeys.ToArray());
                ((IShortcutKeyBinding)pair.NEW.Tag).Global = exchange.Global;
                ((IShortcutKeyBinding)pair.NEW.Tag).IsDisabled = exchange.Disabled;
                ((IShortcutKeyBinding)pair.OLD.Tag).UpdateShortcut(exchange.Shortcut);
                var swap = pair.NEW;
                pair.NEW = pair.OLD;
                pair.OLD = swap;


                UpdateListViewItem(pair.NEW);
                UpdateListViewItem(pair.OLD);

                //pair.NEW.SubItems[1].Text = ShortcutResolver.GetShortcutString(((IShortcutKeyBinding)pair.NEW.Tag).ShortcutKeys);
                //pair.OLD.SubItems[1].Text = ShortcutResolver.GetShortcutString(((IShortcutKeyBinding)pair.OLD.Tag).ShortcutKeys);


                SettingsChanged?.Invoke(pair, new SettingsChangedEventArgs(oldsum));
                return temp;
            }
        }

        public class ShortcutState
        {
            public Keys[] Shortcut { get; set; }
            public bool Global { get; set; }
            public bool Disabled { get; set; }

            public ShortcutState(IEnumerable<Keys> shortcut, bool global, bool disabled)
            {
                Shortcut = shortcut.ToArray();
                Global = global;
                Disabled = disabled;
            }

        }
    }
}
