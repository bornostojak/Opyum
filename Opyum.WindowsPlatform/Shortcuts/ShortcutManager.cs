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
        internal static void SetUpShortcutsOnRequest(object arg, EventArgs e)
        {
            var info = Assembly.GetExecutingAssembly().GetTypes().SelectMany(i => i.GetMethods().Where(m => m.GetCustomAttributes(typeof(OpyumShortcutMethodAttribute)).Count() > 0)).ToArray();

            MethodInfo method = info.Where(m => m.GetCustomAttribute<OpyumShortcutMethodAttribute>().Command == ((IShortcutKeyBinding)arg).Command).FirstOrDefault();
            if (method != null)
            {
                ((IShortcutKeyBinding)arg).AddFunction(Delegate.CreateDelegate(typeof(ShortcutKeyBinding.DELEGATE), null, method));
                ((IShortcutKeyBinding)arg).Description = ((OpyumShortcutMethodAttribute)method.GetCustomAttribute(typeof(OpyumShortcutMethodAttribute))).Description;
                ((IShortcutKeyBinding)arg).Action = ((OpyumShortcutMethodAttribute)method.GetCustomAttribute(typeof(OpyumShortcutMethodAttribute))).Action;
            }
        }

        internal static List<ShortcutKeyBinding> GetShortcutsFromAssembliesInexecutingFolder()
        {
            var directory = Path.GetDirectoryName(Uri.UnescapeDataString((new UriBuilder(Assembly.GetExecutingAssembly().CodeBase)).Path));
            var files = Directory.CreateDirectory(directory).GetFiles(searchPattern: "*.dll", searchOption: SearchOption.AllDirectories).Where(a => a.FullName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))?.Select(u => u.FullName);
            //List<MethodInfo> sclist = new List<MethodInfo>();

            List<ShortcutKeyBinding> list = new List<ShortcutKeyBinding>();
            foreach (var file in files)
            {
                try
                {
                    list.AddRange(Assembly.LoadFile(file)
                        ?.GetTypes()
                        ?.SelectMany(t => t?.GetMethods()
                            ?.Where(m => m?.GetCustomAttribute<OpyumShortcutMethodAttribute>() != null && !(bool)SettingsManager.GlobalSettings?.Shortcuts?.Exists(x => x.Command == m.GetCustomAttribute<OpyumShortcutMethodAttribute>()?.Command)))
                        ?.Select(s => GetKeybindingFromMethodInfo(s))
                        ?.ToList());
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

            list?.ForEach(a =>
            {
                a.Shortcut.Clear();
                a.ShortcutKeys.Clear();
            });

            return list;
        }

        internal static List<ShortcutKeyBinding> GetUdatedShortcuts(string path)
        {
            string fileText = SettingsInterpreter.GetJsonFormFile(path);
            var temp = new List<ShortcutKeyBinding>(JsonConvert.DeserializeObject<SettingsContainer>(fileText).Shortcuts);

            //clone all the temp ones
            return temp.Select(x => new ShortcutKeyBinding() { Command = x.Command, Shortcut = x.Shortcut }).ToList();
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
                return ShortcutKeyBinding.GenerateKeyBinding(atr.Command, atr.DefaultShortcut, atr.Action, atr.Description);
            }
        }
    }
}
