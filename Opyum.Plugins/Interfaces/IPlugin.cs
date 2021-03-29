using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Opyum.Plugins
{
    public interface IPlugin
    {
        String Name { get; }

        String Author { get; }

        String Company { get; }

        String Copyright { get; }

        Boolean Load();
    }
}
