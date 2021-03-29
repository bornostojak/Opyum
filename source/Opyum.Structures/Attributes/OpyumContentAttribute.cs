using System;
using Opyum.Structures.Playlist;

namespace Opyum.Structures.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class OpyumContentAttribute : Attribute
    {
        public OpyumContentAttribute(SourceType type, string name)
        {
            _type = type;
            Name = name;
        }

        private SourceType _type { get; set; }
        public SourceType Type { get => _type; }

        //private string _name = String.Empty;
        //public string Name { get => _name; set { _name = value; } }

        public string Name { get; private set; } = String.Empty;

        public string Descriptopn { get; set; } = String.Empty;
    }
}
