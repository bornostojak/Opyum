using Newtonsoft.Json;
using Opyum.WindowsPlatform.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows.Forms;

namespace Opyum.WindowsPlatform.Shortcuts
{
    public class ShortcutKeyBinding : IShortcutKeyBinding, IDisposable, ISettingsElement<IShortcutKeyBinding>
    {
        [JsonIgnore]
        static KeysConverter kc = new System.Windows.Forms.KeysConverter();

        /// <summary>
        /// The string linking the Method and the <see cref="ShortcutKeyBinding"/>
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// The <see cref="System.Collections.Generic.List{T}"/> of <see cref="Keys"/> the make up the shortcut.
        /// </summary>
        [JsonIgnore]
        public List<Keys> ShortcutKeys { get => _shortcutKeys; set { _shortcutKeys = value; ShortcutChanged?.Invoke(this, new EventArgs()); /* updateShortcutString(); */} }
        List<Keys> _shortcutKeys = new List<Keys>();


        /// <summary>
        /// The shortcut in string form.
        /// </summary>
        public string[] Shortcut { get => ShortcutKeys.Select(x => kc.ConvertToString(null, CultureInfo.CurrentCulture, x)).ToArray(); set { getShortcutFromShortctString(value); } }
        //private List<string> _shortcut = null;

        /// <summary>
        /// Is set when the particular keybing is disabled
        /// </summary>
        [JsonIgnore]
        public bool IsDisabled { get; set; } = false;

        /// <summary>
        /// Is set when the particular keybing is a global shortcut
        /// </summary>
        public bool Global { get; set; }

        /// <summary>
        /// The Description of the shortcut
        /// </summary>
        [JsonIgnore]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// What action the shortcut will take when it has been called.
        /// <para>i.e. Making the window fullscreen etc.</para>
        /// </summary>
        [JsonIgnore]
        public string Action { get; set; }

        /// <summary>
        /// The <see cref="object"/> containing the function delegate
        /// </summary>
        [JsonIgnore]
        public object Function { get; set; }

        /// <summary>
        /// The arguments of the shortcut
        /// </summary>
        public List<string> Args { get; set; } = null;




        public delegate void DELEGATE(string[] args = null);
        //public event EventHandler FunctionRequest;

        public event EventHandler ShortcutChanged;



        //private void checkForShortcutUpdates()
        //{
        //    _shortcut?.ForEach(x =>
        //    {
        //        x = x.Replace(" ", string.Empty);
        //    });
        //    getShortcutFromShortctString();
        //}




        public ShortcutKeyBinding()
        {
            //_shortcutKeys.Add(Keys.None);
            //FunctionRequest += ShortcutManager.SetUpShortcutsOnRequest;
        }

        ~ShortcutKeyBinding()
        {
            this.Dispose();
        }

        //protected void updateShortcutString()
        //{
        //    var kc = new KeysConverter();
        //    _shortcut.Clear();
        //    ShortcutKeys.ForEach(x => _shortcut.Add(kc.ConvertToString(x)));
        //}

        /// <summary>
        /// Updates the shortcut string
        /// </summary>
        /// <param name="shortcut"></param>
        public void UpdateShortcut(IEnumerable<Keys> shortcut)
        {
            ShortcutKeys = new List<Keys>(shortcut);
        }

        protected void getShortcutFromShortctString(IEnumerable<string> str)
        {
            //if the shortcut filed in the Shortcuts.json file is left empty or says "disable", leave the ShortcutKeys to be Keys.None, and if needed set the IsDisabled flag
            if (str == null || (str.Count() == 1 && (IsDisabled = str.Contains("disable"))) || str.Count() == 0)
            {
                return;
            }

            //clear any tresent shortcuts
            _shortcutKeys?.Clear();
            List<Keys> keys = new List<Keys>();

            //for each shortcut in the list of text shortcuts update the ShortcutKeys
            foreach (string scut in str)
            {
                //make sure that two '+' don't get confused with the key '+'
                string text = scut == "+" ? "\\plus" : scut.Replace("++", "+\\plus").Replace("Num+", "NumPlus");
                Keys temp = 0;

                //iterate through each key of the shortcut text and remove the binding char '+'
                foreach (string key in text.Split('+'))
                {
                    try
                    {
                        //or each Keys togetehr into one enum that can later be used for comaprison
                        temp |= (System.Windows.Forms.Keys)kc.ConvertFromString(checkSpecialKeys(key));
                    }
                    catch
                    {
                        //log the error later
                    }
                }
                keys.Add(temp);
            }

            ShortcutKeys = keys;
            //FunctionRequest?.Invoke(this, new EventArgs());
        }

        ///// <summary>
        ///// Set the function the shortcut will call when needed.
        ///// </summary>
        ///// <param name="func"></param>
        //public void AddFunction(object func)
        //{
        //    Function = func;
        //}

        
        /// <summary>
        /// Run the <see cref="System.Action"/> <see cref="Function"/> from.
        /// </summary>
        public void Run()
        {
            ((DELEGATE)Function)?.Invoke(Args?.ToArray());
        }

        /// <summary>
        /// Run the funtion in the keybingargument from the <see cref="object"/> <paramref name="callObject"/>
        /// </summary>
        /// <param name="callObject">The object to invoke the <see cref="System.Action"/> from.</param>
        public void Run(object callObject)
        {
            if (Function != null && callObject != null)
            {
                ((DELEGATE)Delegate.CreateDelegate(typeof(DELEGATE), callObject, ((DELEGATE)Function).GetMethodInfo()))?.Invoke(Args?.ToArray());
            }
        }

        /// <summary>
        /// See if the shortcut in <paramref name="keys"/> match the shortcuts in the <see cref="ShortcutKeyBinding"/>
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool MatchShortcuts(List<Keys> keys)
        {
            if (keys.Count != ShortcutKeys.Count)
            {
                return false;
            }
            return keys.SequenceEqual(_shortcutKeys);
        }

        /// <summary>
        /// Clone the element into a new instance with same parameters
        /// </summary>
        /// <returns></returns>
        public IShortcutKeyBinding Clone()
        {
            return new ShortcutKeyBinding()
            {
                Command = this.Command?.ToString(),
                _shortcutKeys = new List<Keys>(this._shortcutKeys),
                IsDisabled = this.IsDisabled,
                Global = this.Global,
                Action = this.Action?.ToString(),
                Description = this.Description
            };
        }

        /// <summary>
        /// Loads the shortcut string and args data into the current shortcut key from the provided new one.
        /// <para>It then returns itself.</para>
        /// </summary>
        /// <param name="keybinding"></param>
        public IShortcutKeyBinding UpdateDataFromKeybinding(IShortcutKeyBinding keybinding)
        {
            ShortcutKeys = new List<Keys>(keybinding.ShortcutKeys);
            Global = keybinding.Global;
            IsDisabled = keybinding.IsDisabled;
            Args = keybinding.Args == null ? null : new List<string>(keybinding.Args);
            return this;
        }


        #region Disposable


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._shortcutKeys?.Clear();
                this._shortcutKeys = null;
                this.Command = null;
                this.Function = null;
                //this.FunctionRequest = null;
                return;
            }
        }

        /// <summary>
        /// Dispose of this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


        #region Static functions


        //public static IShortcutKeyBinding FindByCommand(string command) => ShortcutKeyBinding.All.Where(x => x.Command == command).FirstOrDefault();


        #endregion

        public static ShortcutKeyBinding GenerateKeyBinding(string command, string[] shortcut, string action, string description, Delegate kbdelegate = null)
        {
            return new ShortcutKeyBinding() { Command = command, Shortcut = shortcut, Action = action, Description = description , Function = kbdelegate};
        }

        public static ShortcutKeyBinding GenerateKeyBinding(string command, IEnumerable<Keys> shortcuts, string action, string description, Delegate kbdelegate = null)
        {
            return new ShortcutKeyBinding() { Command = command, ShortcutKeys = shortcuts.ToList(), Action = action, Description = description , Function = kbdelegate};
        }


        public override bool Equals(object obj)
        {
            if (!(obj is ShortcutKeyBinding))
                return false;
            var o = (ShortcutKeyBinding)obj;
            return Command == o.Command && ShortcutKeys.SequenceEqual(o.ShortcutKeys);
        }

        public override int GetHashCode()
        {
            return Command.GetHashCode() + ShortcutKeys.Sum(x => (int)x);
        }

        static string checkSpecialKeys(string str)
        {
            switch (str.ToLower())
            {
                case "Backslash":
                    return "OemBackslash";
                case "Clear":
                    return "OemClear";
                case "}":
                    return "OemCloseBrackets";
                case ",":
                    return "Oemcomma";
                case "-":
                    return "OemMinus";
                case "{":
                    return "OemOpenBrackets";
                case ".":
                    return "OemPeriod";
                case "|":
                    return "OemPipe";
                case "+":
                    return "Oemplus";
                case "\\plus":
                    return "Oemplus";
                case "?":
                    return "OemQuestion";
                case "\"":
                    return "OemQuotes";
                case ";":
                    return "OemSemicolon";
                case "~":
                    return "Oemtilde";
                case "*":
                    return "Multiply";
                case "<":
                    return "Oem102";
                case "č":
                    return "Oem1";
                case "\'":
                    return "Oem2";
                case "‚":
                    return "Oem3";
                case "š":
                    return "Oem4";
                case "ž":
                    return "Oem5";
                case "đ":
                    return "Oem6";
                case "ć":
                    return "Oem7";
                case "numminus":
                    return "Subtract";
                case "num-":
                    return "Subtract";
                //case "num+":
                //    return "Add";
                case "numplus":
                    return "Add";
                case "num/":
                    return "Divide";
                default:
                    return str;
            }
        }
    }
}
