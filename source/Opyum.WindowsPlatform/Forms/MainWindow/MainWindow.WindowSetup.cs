using Autofac;
using System;
using System.Drawing;
using Opyum.WindowsPlatform.Settings;
using Opyum.WindowsPlatform.Attributes;
using System.Windows.Forms;

namespace Opyum.WindowsPlatform
{
    partial class MainWindow
    {
        //Autofac scope
        private static ILifetimeScope _scope;
        static ILifetimeScope Scope { get { return _scope; } }


        void WindowSetup()
        {
            //Autofac setup
            //IContainer build = ContainerConfig.Configure();
            //_scope = build.BeginLifetimeScope();

            //Load settings from file
            SettingsManager.LoadSettings();
            SettingsManager.GlobalSettings.SettingsChanged += UpdateMenuStrip;

            //Bind all shortcuts
            //KeyBindingArgument.AllBindingsSetup(SettingsInterpreter.SettingsXML.Clone());
            //SetUpShortcuts();


            //Size Settings
            this._oldSize = this.Size;
            //this.SizeChanged += SizeSettings;
            this.SizeChanged += PanelSizeAdaptation;

            //File wather that checks if the settings files have been changed
            SettingsFileSystemWatcherSetup();

            //Back
            this.BackColor = Color.AliceBlue;

            //MenuStrip Settings
            this.MenuStripSetup();
            
            //go fullscreen
            //FullScreenModeChange();

            //Updates the shortcut for the MenuStrip buttons
            UpdateMenuStrip();

        }

        private void PanelSizeAdaptation(object sender, EventArgs e)
        {
            double WindowOfsetRatio = 1.82;

            if (((double)this.Width / (double)this.Height) < WindowOfsetRatio)
            {
                panel2.Width = (int)((double)this.Width / 4.8); 
            }
            panel2.Width = panel2.Width < 300 ? 300 : panel2.Width;

            panel1.Height = (int)((double)this.Height / 7.2);
            panel1.Height = panel1.Height < 120 ? 120 : panel1.Height;
            
        }

        [OpyumShortcutMethod("hide_panels", Description = "Shows or hides the main panels.", Action = "Hide panels")]
        public void Kill(string[] args = null)
        {
            panel1.Visible = !panel1.Visible;
            panel2.Visible = !panel2.Visible;
            //panel2.Visible = !panel2.Visible;
        }
    }
}
