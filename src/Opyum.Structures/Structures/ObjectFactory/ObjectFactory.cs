using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.Structures.Structures
{
    /// <summary>
    /// The class tasked with object instantiation
    /// </summary>
    public static class ObjectFactory
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();



            return builder.Build();
        }
    }
}
