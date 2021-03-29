using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Opyum.WindowsPlatform
{
    public partial class MainWindow
    {
        /// <summary>
        /// Method is called when the windows is trying to be closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnClosing_Safety_Question(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show(this, "Are you sure you want to CLOSE this program?", "WARNING: Closing Program", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

            switch (dr)
            {
                case DialogResult.None:
                    e.Cancel = true;
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    e.Cancel = false;
                    break;
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    e.Cancel = true;
                    break;
            }

        }
    }
}
