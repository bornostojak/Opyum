using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Opyum.WindowsPlatform
{
    partial class MainWindow
    {

        //////////////////////////////////////////    VARS    //////////////////////////////////////////




        ///////////////////////////////////////    STATIC VARS    ///////////////////////////////////////




        ///////////////////////////////////////    CONSTRUCTOR    ///////////////////////////////////////




        ///////////////////////////////////      STATIC METHODS      ///////////////////////////////////




        ///////////////////////////////////////      METHOD      ///////////////////////////////////////

        /// <summary>
        /// Inserts the form given to this method into the Main Window
        /// </summary>
        /// <param name="form"></param>
        public void AddWindow(Form form)
        {
            form.MdiParent = this as Form;
            form.Show();
        }



        ///////////////////////////////////////      EVENTS      ///////////////////////////////////////
    }
}
