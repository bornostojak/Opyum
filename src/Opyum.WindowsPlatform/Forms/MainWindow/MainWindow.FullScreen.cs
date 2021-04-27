using System;
using System.Drawing;
using System.Windows.Forms;
using Opyum.Structures.Attributes;
using Opyum.WindowsPlatform.Settings;
using Opyum.WindowsPlatform.Attributes;
using System.Collections.Generic;

namespace Opyum.WindowsPlatform
{
    public partial class MainWindow
    {
        public enum ScreenMode { None = 0, Normal = 1, FullScreen = 2, Previous = 4 };
        private bool _menustripPreviouslyVisible;

        private ScreenMode _previousScreenViewMode;
        private ScreenMode _screenViewMode;

        public ScreenMode ScreenViewMode
        {
            get
            {
                return _screenViewMode;
            }
            set
            {
                if (value == ScreenMode.Previous)
                {
                    var temp = _previousScreenViewMode;
                    _screenViewMode = _previousScreenViewMode;
                    _previousScreenViewMode = temp;
                }
                else
                {
                    _previousScreenViewMode = _screenViewMode;
                    _screenViewMode = value;
                }
                ScreenModeChanged?.Invoke(this, new EventArgs());
            }
        }


        private Size sizePriorToFullScreen = new Size(50, 50);
        private Point locationPriorToFullScreen;


        public delegate void ScreenModeEvent(object sender, EventArgs e);
        public event ScreenModeEvent ScreenModeChanged;

        //This function is activated when the application is put into fullscreenmode
        [OpyumShortcutMethod("fullscreen", Description = "Switch between fullscreen and windowed view.", Action = "Fullscreen", DefaultShortcut = new[] { Keys.F11 })]
        public void FullScreenModeChange(string[] args = null)
        {
            if (ScreenViewMode == ScreenMode.FullScreen)
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.Size = sizePriorToFullScreen;
                this.Location = locationPriorToFullScreen;
                //IsFullScreen = false;
                UnhideMenuBar();
                OnNormalScreen();
                ScreenViewMode = ScreenMode.Previous;
                MenuStrip.Visible = _menustripPreviouslyVisible;
            }
            else
            {
                _menustripPreviouslyVisible = MenuStrip.Visible;
                MenuStrip.Visible = false;
                sizePriorToFullScreen = this.Size;
                locationPriorToFullScreen = this.Location;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Normal;
                this.Bounds = Screen.PrimaryScreen.Bounds;
                //IsFullScreen = true;
                HideMenuBar();
                OnFullScreen();
                ScreenViewMode = ScreenMode.FullScreen;
            }
        }
        public void FullScreenModeChange(object sender, EventArgs e) => FullScreenModeChange();

        public void OnFullScreen()
        {
            //MenuStrip.Hide();
        }

        public void OnNormalScreen()
        {
            //MenuStrip.Show();
        }
    }
}
