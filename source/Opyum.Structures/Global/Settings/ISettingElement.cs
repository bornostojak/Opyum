using System.Collections.Generic;

namespace Opyum.Structures.Global.Settings
{
    public interface ISettingElement<T>
    {
        string Key { get; }
        object Type { get; }
        List<T> Values { get; set; }
    }

    public interface ISettingElement
    {
        string Key { get; }
        object Type { get; }
        List<object> Values { get; set; }
    }
}