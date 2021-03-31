using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Opyum.Structures.Attributes;
using Opyum.WindowsPlatform.Plugins;
using Opyum.WindowsPlatform.Shortcuts;

namespace Opyum.WindowsPlatform.Settings
{
    public class SettingsContainer : IDisposable
    {
        [OpyumSettingsGroup("General")]
        public ShortcutKeyBinding[] Shortcuts { get => _shortcuts.Cast<ShortcutKeyBinding>().ToArray(); set { updateShortcuts(value);} }
        List<IShortcutKeyBinding> _shortcuts = new List<IShortcutKeyBinding>();
        Dictionary<Keys[], IShortcutKeyBinding> _shortcutslist = new Dictionary<Keys[], IShortcutKeyBinding>(new ShortcutDictonayComparer());

        [OpyumSettingsGroup("Appearance")]
        public List<ColorContainer> Colors { get; set; } = null;
        [OpyumSettingsGroup("Plugins")]
        public PluginManager Plugins { get; private set; }

        
        

        ~SettingsContainer()
        {
            this.Dispose();
        }


        public SettingsContainer Clone()
        {
            return new SettingsContainer()
            {
                _shortcuts = this?._shortcuts?.Select(x => x.Clone()).ToList(),
                Colors = this.Colors?.Select(h => h.Clone()).ToList()
            };
        }

        public void Update(SettingsContainer update)
        {
            updateShortcuts(update.Shortcuts);
            Colors = update.Colors;
            Plugins = update.Plugins;
        }

        public IShortcutKeyBinding FindShortcut(Keys[] keys) => keys != null && _shortcutslist.ContainsKey(keys) ? _shortcutslist[keys] : null;

        /// <summary>
        /// Updates the shortcuts
        /// </summary>
        /// <param name="shortcuts"></param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        private void updateShortcuts(IEnumerable<IShortcutKeyBinding> shortcuts)
        {
            if (shortcuts != null && shortcuts.Count() > 0)
            {
                var old = _shortcuts.ToDictionary(j => j.Command);
                var notpresent = shortcuts.GroupBy(c => old.ContainsKey(c.Command), c => c, (there, keys) => new { Found = there, Keys = keys });

                foreach (var item in notpresent)
                {
                    if (item.Found)
                    {
                        foreach (var i in item.Keys)
                        {
                            old[i.Command].UpdateShortcut(i.ShortcutKeys);
                        }
                    }
                    else
                    {
                        ShortcutManager.SetUpShortcuts(item.Keys);
                        _shortcuts.AddRange(item.Keys);
                    }
                }

                _shortcutslist = _shortcuts.Where(s => s.ShortcutKeys.Count() > 0).ToDictionary(x => x.ShortcutKeys.ToArray(), new ShortcutDictonayComparer());

                //check if a shortcut combination will result with premature shortcut resolution
                //will a 3 key combo result in a 2 key combo resultion
                //i.e. Ctrl+K, Ctrl+P might be resolved with Ctrk+K to wrong function
                var zone = _shortcuts.GroupBy(s => s.ShortcutKeys.Count(), s => s, (count, s) => new { Count = count, Shortcuts = s }).OrderByDescending(i => i.Count).ToArray();
                if (zone.Where(u => u.Count > 1).SelectMany(q => q.Shortcuts.TakeWhile(s => _shortcutslist.ContainsKey(s.ShortcutKeys.Take(q.Count - 1).ToArray()))).Count() > 0)
                {
                    throw new ArgumentOutOfRangeException("There is a shortcut key combindation that can resolve before the shortcut can be reached");
                }
            }
        }

        

        /// <summary>
        /// Get the value of a property by giving the properties name
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public object this[string property]
        {
            get => this.GetType().GetProperty(property)?.GetValue(this);
            set { if (value?.GetType() == this.GetType().GetProperty(property).PropertyType || value == null) this.GetType().GetProperty(property).SetValue(this, value); }
        }


        #region Disposing

        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }

        private void Dispose(bool disposing)
        {
            if (disposing && Shortcuts != null)
            {
                foreach (var item in Shortcuts)
                {
                    item?.Dispose();
                }
                Shortcuts = null;
            }
        } 
        #endregion
    }

    public class ShortcutDictonayComparer : IEqualityComparer<Keys[]>
    {
        public bool Equals(Keys[] x, Keys[] y) => x.SequenceEqual(y);

        public int GetHashCode(Keys[] obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            unchecked
            {
                var ksum = obj.Sum(b => (int)b);
                return ksum;
            }
        }
    }
}
