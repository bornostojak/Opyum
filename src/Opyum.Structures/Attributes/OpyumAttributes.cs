using System;

namespace Opyum.Structures
{
    
    public class OpyumAttributes
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
        public class UIModule : Attribute
        {
            public string Parent { get; set; }
        }
    }
}
