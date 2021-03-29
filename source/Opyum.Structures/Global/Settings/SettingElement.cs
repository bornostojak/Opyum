using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.Structures.Global.Settings
{
    public class SettingElement<T> : ISettingElement<T>
    {
        public virtual string Key { get; protected internal set; }
        public virtual List<T> Values { get; set; } = new List<T>();
        public virtual object Type { get; protected internal set; }

        public SettingElement(string key, T value)
        {
            Key = key;
            Values.Add(value);
        }
    }

    public class SettingElement : SettingElement<object>, ISettingElement
    {
        public SettingElement(string key, object value) : base(key, value)
        {

        }
    }
}
