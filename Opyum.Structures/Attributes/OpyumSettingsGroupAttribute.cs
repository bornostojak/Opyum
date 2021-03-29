using System;

namespace Opyum.Structures.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OpyumSettingsGroupAttribute : Attribute
    {
        public string Group { get; set; } = string.Empty;

        public OpyumSettingsGroupAttribute(string group)
        {
            Group = group;
        }
    }
}
