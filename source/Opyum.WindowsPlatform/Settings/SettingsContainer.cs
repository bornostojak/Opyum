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
        Dictionary<Keys[], IShortcutKeyBinding> _shortcutsdict = new Dictionary<Keys[], IShortcutKeyBinding>(new ShortcutDictonayComparer());

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

        public IShortcutKeyBinding FindShortcut(Keys[] keys) => keys != null && _shortcutsdict.ContainsKey(keys) ? _shortcutsdict[keys] : null;

        /// <summary>
        /// Updates the shortcuts
        /// </summary>
        /// <param name="shortcuts"></param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        private void updateShortcuts(IEnumerable<IShortcutKeyBinding> shortcuts)
        {
            //continue if there are shortcuts to update
            if (shortcuts != null && shortcuts.Count() > 0)
            {
                //hashmap of current shortcuts
                var old = _shortcuts.ToDictionary(j => j.Command);

                //
                var groupbypresent = shortcuts.GroupBy(c => old.ContainsKey(c.Command) ? (old[c.Command].ShortcutKeys.SequenceEqual(c.ShortcutKeys) ? 2 : 1) : 0, c => c, (gr, keys) => new { Group = gr, Keys = keys });

                foreach (var item in groupbypresent)
                {
                    if (item.Group == 2)
                    {
                        continue;
                    }
                    if (item.Group == 1)
                    {
                        foreach (var i in item.Keys)
                        {
                            old[i.Command].UpdateShortcut(i.ShortcutKeys);
                        }
                    }
                    else if(item.Group == 0)
                    {
                        ShortcutManager.SetUpShortcuts(item.Keys);
                        _shortcuts.AddRange(item.Keys);
                    }
                }

                _shortcutsdict = _shortcuts.Where(s => s.ShortcutKeys.Count() > 0).ToDictionary(x => x.ShortcutKeys.ToArray(), new ShortcutDictonayComparer());

                var zone = _shortcuts.GroupBy(s => s.ShortcutKeys.Count(), s => s, (count, s) => new { Count = count, Shortcuts = s }).OrderByDescending(i => i.Count).ToArray();
                if (zone.Where(u => u.Count > 1).SelectMany(q => q.Shortcuts.TakeWhile(s => _shortcutsdict.ContainsKey(s.ShortcutKeys.Take(q.Count - 1).ToArray()))).Count() > 0)
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
