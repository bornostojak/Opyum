using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.Structures.Playlist
{
    /// <summary>
    /// The <see cref="ContentCreatorAssistant"/> is used to crearte classes that the <see cref="ContentCreator"/>
    /// will later look for in order to know how to create a <see cref="Content"/> from a given source location.
    /// </summary>
    [Opyum.Structures.Playlist.Attributes.ContentCreatorAssistant]
    public abstract class ContentCreatorAssistant
    {
        /// <summary>
        /// Returns the extensioins the <see cref="ContentCreatorAssistant"/> supports.
        /// </summary>
        public virtual string[] Extensions { get; private set; }

        public abstract bool DoesSupportSource(string source);

        /// <summary>
        /// Returns the correct <see cref="ContentInterpreter"/> for a given file's format.
        /// </summary>
        /// <returns></returns>
        public abstract ContentInterpreter GetContetnInterpreter();
    }
}
