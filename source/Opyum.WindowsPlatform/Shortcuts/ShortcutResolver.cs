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

namespace Opyum.WindowsPlatform.Settings
{
    public static class ShortcutResolver
    {
        static object callerObject { get; set; }
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
        public static void ResolveShortcut(object sender, KeyEventArgs e, List<ShortcutKeyBinding> shortcuts)
        {
            var value = e.KeyData & ~(Keys.Modifiers | Keys.ControlKey | Keys.Menu | Keys.RButton | Keys.LButton);
            if (value == 0)
            {
                return;
            }
            callerObject = sender;
            KeysPressed.Add(e.KeyData);
            compareShortcutSequence(shortcuts);
        }

        /// <summary>
        /// Get the string of the shortcut pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetShortcutString(object sender, KeyEventArgs e)
        {
            var value = e.KeyData & ~(Keys.Modifiers | Keys.ControlKey | Keys.Menu | Keys.RButton | Keys.LButton);
            if (value == 0)
            {
                return _scString;
            }
            KeysConverter kc = new KeysConverter();
            _scString += (_scString == string.Empty ? "" : ", ") + ((e.KeyData & Keys.Control) != 0 ? "Ctrl+" : "") + ((e.KeyData & Keys.Alt) != 0 ? "Alt+" : "") + ((e.KeyData & Keys.Shift) != 0 ? "Shift+" : "") + kc.ConvertToString(null, CultureInfo.CurrentCulture, e.KeyCode);

            runKeysPressedTimer();
            return _scString;
        }

        /// <summary>
        /// Get the string of the shortcut pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string GetShortcutString(object sender, List<Keys> keys)
        {
            KeysConverter kc = new KeysConverter();
            try
            {
                return string.Join(", ", keys?.Select(k => kc.ConvertToString(null, CultureInfo.CurrentCulture, k)));
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
            KeysConverter kc = new KeysConverter();
            try
            {
                return keys?.Select(k => kc.ConvertToString(null, CultureInfo.CurrentCulture, k)).ToList();
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
            if (KeysPressed.Count > 0 && _shortcutCompareList.Count > 0)
            {
                try
                {
                    if (callerObject is Control && ((Control)callerObject).InvokeRequired && KeysPressed != null & KeysPressed.Count > 0)
                    {
                        ((Control)callerObject).Invoke(new MethodInvoker(() => _shortcutCompareList?.Where(a => a.ShortcutKeys.SequenceEqual(KeysPressed))?.FirstOrDefault()?.Run(callerObject)));
                        KeysPressed.Clear();
                    }
                }
                catch (InvalidOperationException)
                {
                    
                };
            }
            _keysPressedTimer.Stop();
            _keysPressedTimer.Dispose();
            _keysPressedTimer = new System.Timers.Timer(timerDuration);
            KeysPressed.Clear();
            _shortcutCompareList = null;
            _keysPressedTimerSet = false;
            _scString = string.Empty;
        }

        static void compareShortcutSequence(List<ShortcutKeyBinding> globalShortcuts)
        {
            //check if there are already some detected keys pressed
            if (_shortcutCompareList == null)
            {
                _shortcutCompareList = globalShortcuts?.Where(i => i.ShortcutKeys?.Count > 0 && i.ShortcutKeys?.First() == KeysPressed.First())?.ToList();
            }
            //if there are already some keys pressed, shorten the number of potential methods to summon.
            else
            {
                _shortcutCompareList = _shortcutCompareList?.Where(i => KeysPressed.Where(x => i.ShortcutKeys.Contains(x)).Count() == KeysPressed.Count).ToList();
            }
            //if there are none potentila KeyBindings
            if (_shortcutCompareList == null || _shortcutCompareList.Count == 0)
            {
                KeysPressed.Clear();
                _shortcutCompareList = null;
                return;
            }
            else if (_shortcutCompareList.Count() == 1 && KeysPressed.SequenceEqual(_shortcutCompareList.First().ShortcutKeys))
            {
                _shortcutCompareList?.FirstOrDefault()?.Run(callerObject);
            }
            else
            {
                runKeysPressedTimer();
                return;
            }
            KeysPressed.Clear();
            _shortcutCompareList = null;
            return;
        }

        
    }
}
