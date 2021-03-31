using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.WindowsPlatform.Settings
{
    public partial class SettingsEditor
    {
        Dictionary<object, int> ChangeLog = new Dictionary<object, int>();
        Dictionary<object, int> _recordedChanges = null;

        public void OKButton_Action(object sender, EventArgs e)
        {
            ApplyButton_Action(sender, e);
            CancelButton_Action(sender, e);
        }

        public void ApplyButton_Action(object sender, EventArgs e)
        {
            SettingsManager.GlobalSettings.Update(NewSettings);
            NewSettings = NewSettings.Clone();
            reloadPanels();
            SettingsManager.SaveSettings();
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
            this.Close();
        }

        public void CheckForChanges(object sender, SettingsChangedEventArgs e)
        {
            if (!ChangeLog.ContainsKey(sender))
            {
                ChangeLog.Add(sender, e.UnsavedSettingsCount);
            }
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
        }
    }
}
