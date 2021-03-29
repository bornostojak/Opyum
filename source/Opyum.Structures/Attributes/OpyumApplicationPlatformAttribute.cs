using System;

namespace Opyum.Structures.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OpyumApplicationPlatformAttribute : Attribute
    {
        public Enums.ApplicationPlatform Platform { get; private set; }

        public OpyumApplicationPlatformAttribute(Enums.ApplicationPlatform platform)
        {
            Platform = platform;
        }
    }
    
}
