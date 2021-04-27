using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Opyum.Launcher
{
    static partial class Program
    {
        private static object FindApplicatinoForm()
        {

            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string dir = System.IO.Path.GetDirectoryName(path);

            List<Assembly> assemblies = new List<Assembly>();

            foreach (string dll in System.IO.Directory.GetFiles(dir))
            {
                Assembly t;
                try
                {
                    t = Assembly.LoadFile(dll);
                }
                catch (Exception)
                {
                    continue;
                }
                assemblies.Add(t);
            }


            return assemblies.SelectMany(a => a.GetTypes())
                                  .Where(a => a.GetCustomAttributes<Opyum.Structures.Attributes.OpyumApplicationPlatformAttribute>().Count() > 0)
                                  .Where(a => ((Opyum.Structures.Attributes.OpyumApplicationPlatformAttribute)a.GetCustomAttribute<Opyum.Structures.Attributes.OpyumApplicationPlatformAttribute>()).Platform == Structures.Enums.ApplicationPlatform.Windows)
                                  .FirstOrDefault();
        }
    }
}
