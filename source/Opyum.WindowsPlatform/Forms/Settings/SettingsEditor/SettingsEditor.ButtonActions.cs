using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Opyum.WindowsPlatform.Settings
{
    public partial class SettingsEditor
    {
        Dictionary<object, int> ChangeLog = new Dictionary<object, int>();
        Dictionary<object, int> _recordedChanges = null;

        public async void OKButton_Action(object sender, EventArgs e)
        {
            await applyChanges();
            CancelButton_Action(sender, e);
        }

        public async void ApplyButton_Action(object sender, EventArgs e)
        {
            await applyChanges();
        }

        private async Task applyChanges()
        {
            await SettingsManager.GlobalSettings.UpdateAsync(NewSettings.Clone());
            //reloadPanels();
            SettingsInterpreter.SaveSettings(SettingsManager.GlobalSettings);
            _recordedChanges = new Dictionary<object, int>(ChangeLog);
            applyButton.Enabled = false;
        }

        private void reloadPanels()
        {
            foreach (var item in ContentPanel.Controls)
            {
                if (item is ISettingsPanel)
                {
                    ((ISettingsPanel)item).LoadElements();
                }
            }
        }

        public void CancelButton_Action(object sender, EventArgs e)
        {
            //check if changes were made and if yes, ask if they realy want to cancel
            if (_recordedChanges == null ? ChangeLog.Values?.Sum() > 0 : !_recordedChanges.SequenceEqual(ChangeLog))
            {
                var result = MessageBox.Show("There are unsaved changes.\nExit without saving?", "WARNINIG: Unsaved changes detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }
            this.Close();
        }

        public void CheckForChanges(object sender, SettingsChangedEventArgs e)
        {
            //it he changelog doesnt have the used setting change tracked, add & update it
            if (!ChangeLog.ContainsKey(sender))
            {
                ChangeLog.Add(sender, e.UnsavedSettingsCount);
            }
            //otherwise just update it
            else
            {
                ChangeLog[sender] = e.UnsavedSettingsCount;
            }

            if (ChangeLog.Values?.Sum() > 0)
            {
                applyButton.Enabled = true;
            }
            else
            {
                applyButton.Enabled = false;
            }
            if (_recordedChanges != null)
            {
                applyButton.Enabled = !_recordedChanges.SequenceEqual(ChangeLog);
            }
            //BETTWR WAY: create a undoredostack status function that returns a number inidcating what state the stack is
            //if the numbers matches the recorded state number, no changes are untracked
        }
    }
}
