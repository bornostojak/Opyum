using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace Opyum.WindowsPlatform.Shortcuts
{
    public interface IShortcutKeyBinding : IDisposable
    {
        /// <summary>
        /// The string linking the Method and the <see cref="IShortcutKeyBinding"/>
        /// </summary>
        string Command { get; set; }

        /// <summary>
        /// The <see cref="List{T}"/> of <see cref="Keys"/> the make up the shortcut.
        /// </summary>
        List<Keys> ShortcutKeys { get; set; }

        /// <summary>
        /// The <see cref="object"/> containing the function delegate
        /// </summary>
        object Function { get; }

        /// <summary>
        /// The shortcut in string form.
        /// </summary>
        List<string> Shortcut { get; set; }

        /// <summary>
        /// The Description of the shortcut
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// What action the shortcut will take when it has been called.
        /// <para>i.e. Making the window fullscreen etc.</para>
        /// </summary>
        string Action { get; set; }

        /// <summary>
        /// Is set when the particular keybing is disabled
        /// </summary>
        bool IsDisabled { get; set; }

        /// <summary>
        /// Is set when the particular keybing is a global shortcut
        /// </summary>
        bool Global { get; set; }

        /// <summary>
        /// The arguments of the shortcut
        /// </summary>
        List<string> Args { get; set; }

        /// <summary>
        /// Set the function the shortcut will call when needed.
        /// </summary>
        /// <param name="func"></param>
        void AddFunction(object func);

        /// <summary>
        /// See if the shortcut in <paramref name="keys"/> match the shortcuts in the <see cref="IShortcutKeyBinding"/>
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        bool MatchShortcuts(List<Keys> keys);

        /// <summary>
        /// Run the <see cref="System.Action"/> <see cref="Function"/> from.
        /// </summary>
        void Run();

        /// <summary>
        /// Run the funtion in the keybingargument from the <see cref="object"/> <paramref name="callObject"/>
        /// </summary>
        /// <param name="callObject">The object to invoke the <see cref="System.Action"/> from.</param>
        void Run(object callObject);

        /// <summary>
        /// Updates the shortcut string
        /// </summary>
        /// <param name="str"></param>
        void UpdateShortcutString(List<string> str);

        /// <summary>
        /// Clone the element into a new instance with same parameters
        /// </summary>
        /// <returns></returns>
        IShortcutKeyBinding Clone();

        /// <summary>
        /// Loads the shortcut string and args data into the current shortcut key from the provided new one.
        /// <para>It then returns itself.</para>
        /// </summary>
        /// <param name="keybinding"></param>
        IShortcutKeyBinding UpdateDataFromKeybinding(IShortcutKeyBinding keybinding);
    }
}