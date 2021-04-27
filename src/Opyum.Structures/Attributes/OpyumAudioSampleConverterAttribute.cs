using System;

namespace Opyum.Structures.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OpyumAudioSampleConverterAttribute : Attribute
    {
        public OpyumAudioSampleConverterAttribute()
        {

        }

        public string Extention { get; set; }


    }
}