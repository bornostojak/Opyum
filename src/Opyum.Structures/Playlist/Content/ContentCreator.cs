using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.Structures.Playlist
{
    public static class ContentCreator
    {
        internal static List<ContentCreatorAssistant> Assistants;

        public static IContent CreateContent(string source)
        {
            if (System.IO.File.Exists(source))
            {
                var t = FindAssistantForFile(source);
                return null;
            }
            else return null;
        }

        private static IContent ContentSetup(ContentCreatorAssistant a, string source)
        {
            var t = new Content();
            t.Source = source;
            //t.AudioInfo = a.GetAudioInfo();
            //t.Begining = a.GetBegining();
            //t.
            //t.



            return t;
        }

        private static ContentCreatorAssistant FindAssistantForFile(string source)
        {
            return Assistants.Where((x) => x.Extensions.Contains(Path.GetExtension(source))).Where((y) => y.DoesSupportSource(source)).FirstOrDefault();
        }
    }
}