using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.AudioBase.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ContentAttribute : Attribute
    {
        public ContentAttribute(ItemType type, string name)
        {
            _type = type;
            Name = name;
        }

        private ItemType _type { get; set; }
        public ItemType Type { get => _type; }

        //private string _name = String.Empty;
        //public string Name { get => _name; set { _name = value; } }

        public string Name { get; private set; } = String.Empty;

        public string Descriptopn { get; set; } = String.Empty;
    }
}
