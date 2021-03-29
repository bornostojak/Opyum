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
        static object callerObject { get; set; }
        static private KeysConverter kc = new KeysConverter();
        static string _scString = string.Empty;
        static bool _keysPressedTimerSet = false;
        static readonly int timerDuration = 750;
        static System.Timers.Timer _keysPressedTimer = new System.Timers.Timer(timerDuration);
        static List<ShortcutKeyBinding> _shortcutCompareList = null;

        static List<Keys> KeysPressed { get; set; } = new List<Keys>();

        /// <summary>
        /// Runs the method assigned to the shortcut
        /// </summary>
        /// <param name="sender">The obcjet whos event triggered the Mmethod.</param>
        /// <param name="e">The args of the event.</param>
        public static IShortcutKeyBinding ResolveShortcut(object sender, KeyEventArgs e)
        {
            ShortcutKeyBinding result = null;
            var shrct = ShortcutResolver.GrabShortcut(e);
            if (shrct != null)
                result = SettingsManager.GlobalSettings.Shortcuts.FirstOrDefault(x => x.ShortcutKeys.SequenceEqual(shrct));
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
        public static string GetShortcutString(List<Keys> keys)
        {
            if (keys != null)
            {
                return String.Join(", ", GetShortcutStringList(keys));
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

        /// <summary>
        /// Get the string of the shortcut pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static List<string> GetShortcutStringList(List<Keys> keys)
        {
            try
            {
                return keys.Select(x => kc.ConvertToString(null, CultureInfo.CurrentCulture, x)).ToList();
            }
            catch (Exception)
            {
                return new List<string>();
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
            //if (KeysPressed.Count > 0 && _shortcutCompareList?.Count > 0)
            //{
            //    try
            //    {
            //        if (callerObject is Control && ((Control)callerObject).InvokeRequired && KeysPressed != null & KeysPressed.Count > 0)
            //        {
            //            ((Control)callerObject).Invoke(new MethodInvoker(() => _shortcutCompareList?.Where(a => a.ShortcutKeys.SequenceEqual(KeysPressed))?.FirstOrDefault()?.Run(callerObject)));
            //            KeysPressed.Clear();
            //        }
            //    }
            //    catch (InvalidOperationException)
            //    {
                    
            //    };
            //}
            _keysPressedTimer.Stop();
            _keysPressedTimer.Dispose();
            _keysPressedTimer = new System.Timers.Timer(timerDuration);
            KeysPressed.Clear();
            _shortcutCompareList = null;
            _keysPressedTimerSet = false;
            _scString = string.Empty;
        }

    }
}
