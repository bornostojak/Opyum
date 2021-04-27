using System;

namespace Opyum.Structures.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OpyumSettingsPanelElementAttribute : Attribute
    {
        //public Type ParentType { get; set; }
        public Type Type { get; set; }

        public OpyumSettingsPanelElementAttribute(Type type)//, Type parentType)
        {
            Type = type;
            //ParentType = parentType;
        }
    }
}
