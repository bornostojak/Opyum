using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Opyum.WindowsPlatform.Attributes;
using System.IO;
using Opyum.WindowsPlatform.Shortcuts;
using Opyum.Structures.Attributes;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Opyum.WindowsPlatform.Settings
{
    public static class ShortcutManager
    {
        internal static void SetUpShortcuts(IEnumerable<IShortcutKeyBinding> shortcuts)
        {
            var map = shortcuts.ToDictionary(f => f.Command);
            var info = Assembly.GetExecutingAssembly().GetTypes().SelectMany(i => i.GetMethods().Where(m => m.GetCustomAttributes(typeof(OpyumShortcutMethodAttribute)).Count() > 0)).ToArray();

            var methods = info.Where(m => map.ContainsKey(m.GetCustomAttribute<OpyumShortcutMethodAttribute>().Command));
            if (methods != null && methods.Count() > 0)
            {
                foreach (var method in methods)
                {
                    var shortcut = map[method.GetCustomAttribute<OpyumShortcutMethodAttribute>().Command];
                    shortcut.Function = Delegate.CreateDelegate(typeof(ShortcutKeyBinding.DELEGATE), null, method);
                    shortcut.Description = ((OpyumShortcutMethodAttribute)method.GetCustomAttribute(typeof(OpyumShortcutMethodAttribute))).Description;
                    shortcut.Action = ((OpyumShortcutMethodAttribute)method.GetCustomAttribute(typeof(OpyumShortcutMethodAttribute))).Action; 
                }
            }
        }

        internal static ShortcutKeyBinding[] GetShortcutsFromAssembliesInExecutingAssemblies()
        {
            var directory = Path.GetDirectoryName(Uri.UnescapeDataString((new UriBuilder(Assembly.GetExecutingAssembly().CodeBase)).Path));
            var files = Directory.CreateDirectory(directory).GetFiles(searchPattern: "*.dll", searchOption: SearchOption.AllDirectories).Where(a => a.FullName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))?.Select(u => u.FullName);
            //List<MethodInfo> sclist = new List<MethodInfo>();

            IEnumerable<ShortcutKeyBinding> list = null;
            foreach (var file in files)
            {
                try
                {
                    list = Assembly.LoadFile(file)
                        ?.GetTypes()
                        ?.SelectMany(t => t?.GetMethods()
                            ?.Where(m => m?.GetCustomAttribute<OpyumShortcutMethodAttribute>() != null && !(bool)SettingsManager.GlobalSettings?.Shortcuts?.Any(x => x.Command == m.GetCustomAttribute<OpyumShortcutMethodAttribute>()?.Command)))
                        ?.Select(s => GetKeybindingFromMethodInfo(s));
                }
                catch (InvalidOperationException)
                {

                }
                catch (ReflectionTypeLoadException)
                {

                }
                catch (Exception e)
                {

                    throw e;
                } 
            }


            return list.ToArray();
        }

        private static ShortcutKeyBinding GetKeybindingFromMethodInfo(MethodInfo method)
        {
            var atr = method.GetCustomAttribute<OpyumShortcutMethodAttribute>();
            if (atr == null)
            {
                return null;
            }
            else
            {
                var kbdelegate = Delegate.CreateDelegate(typeof(ShortcutKeyBinding.DELEGATE), null, method);
                ShortcutKeyBinding keybinding = ShortcutKeyBinding.GenerateKeyBinding(atr.Command, (string[])null, atr.Action, atr.Description, kbdelegate);
                return keybinding;
            }
        }
    }
}
