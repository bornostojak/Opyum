using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.WindowsPlatform.Settings
{
    public partial class SettingsEditor
    {
        public void OKButton_Action(object sender, EventArgs e)
        {
            ApplyButton_Action(sender, e);
            CancelButton_Action(sender, e);
        }

        public void ApplyButton_Action(object sender, EventArgs e)
        {
            SettingsManager.GlobalSettings = NewSettings;
            SettingsManager.SaveSettings();
        }

        public void CancelButton_Action(object sender, EventArgs e)
        {
            //check if changes were made and if yes, ask if they realy want to cancel
            this.Close();
        }
    }
}
