using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Opyum.WindowsPlatform.Shortcuts;
using System.Xml;
using System.Threading.Tasks;

namespace Opyum.WindowsPlatform.Settings
{
    public static class ShortcutResolver
    {
        static private KeysConverter kc = new KeysConverter();
        static bool _keysPressedTimerSet = false;
        static readonly int timerDuration = 750;
        static System.Timers.Timer _keysPressedTimer = new System.Timers.Timer(timerDuration);

        static List<Keys> KeysPressed { get; set; } = new List<Keys>();

        /// <summary>
        /// Runs the method assigned to the shortcut
        /// </summary>
        /// <param name="sender">The obcjet whos event triggered the Mmethod.</param>
        /// <param name="e">The args of the event.</param>
        public static IShortcutKeyBinding ResolveShortcut(object sender, KeyEventArgs e)
        {
            IShortcutKeyBinding result = null;
            var shrct = ShortcutResolver.GrabShortcut(e);
            if (shrct != null)
            {
                //int len = shrct.Count();
                //var results = SettingsManager.GlobalSettings.Shortcuts.Where(x => x.ShortcutKeys.SequenceEqual(shrct));
                result = SettingsManager.GlobalSettings?.FindShortcut(shrct.ToArray());
            }
                
            if (result != null)
                KeysPressed.Clear();
            return result;
        }
        

        /// <summary>
        /// Runs the method assigned to the shortcut
        /// </summary>
        /// <param name="sender">The obcjet whos event triggered the Mmethod.</param>
        /// <param name="e">The args of the event.</param>
        public static List<Keys> GrabShortcut(KeyEventArgs e)
        {
            var value = e.KeyData & ~(Keys.Modifiers | Keys.ControlKey | Keys.Menu | Keys.RButton | Keys.LButton);
            if (value == 0)
            {
                return null;
            }
            KeysPressed.Add(e.KeyData);
            runKeysPressedTimer();
            return KeysPressed.Cast<Keys>().ToList();
        }

        /// <summary>
        /// Get the string of the shortcut pressed
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string GetShortcutString(IEnumerable<Keys> keys)
        {
            if (keys != null)
            {
                return String.Join(", ", keys.Select(x => GetShortcutString(x)));
            }
            return string.Empty;
        }

        /// <summary>
        /// Get the string of the shortcut pressed
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string GetShortcutString(Keys keys)
        {
            try
            {
                return kc.ConvertToString(null, CultureInfo.CurrentCulture, keys);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        //After a while clears KeysPressed
        static void runKeysPressedTimer()
        {
            if (_keysPressedTimerSet)
            {
                _keysPressedTimer.Stop();
                _keysPressedTimer.Elapsed -= keysPressedTimerEventCall;
                _keysPressedTimer.Dispose();
                _keysPressedTimer = new System.Timers.Timer(timerDuration);
            }

            _keysPressedTimer.Elapsed += keysPressedTimerEventCall; 
            _keysPressedTimer.Start();
            _keysPressedTimerSet = true;
        }

        

        static void keysPressedTimerEventCall(object s, EventArgs e)
        {
            _keysPressedTimer.Stop();
            _keysPressedTimer.Dispose();
            _keysPressedTimer = new System.Timers.Timer(timerDuration);
            KeysPressed.Clear();
            _keysPressedTimerSet = false;
        }

    }
}
