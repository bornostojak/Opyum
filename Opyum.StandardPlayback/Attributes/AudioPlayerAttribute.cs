using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.StandardPlayback.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AudioPlayerAttribute : Attribute
    {
        public AudioPlayerAttribute(string name)
        {
            Name = name;
        }

        public AudioPlayerAttribute(string name, string[] supportedFormats)
        {
            Name = name;
            SupportedFormats.AddRange(supportedFormats);
        }

        public AudioPlayerAttribute(string name, string supportedFormats)
        {
            Name = name;
            SupportedFormats.AddRange(supportedFormats.Replace(" ", string.Empty).Split(';'));
        }

        public string Name { get; set; } = String.Empty;

        public string Version { get; set; } = String.Empty;

        //public Opyum.Structures.Playlist.ContentType Type { get; set; } = Opyum.Structures.Playlist.ContentType.None;

        public List<string> SupportedFormats { get; set; } = new List<string>();
    }
}
