using System.IO;

namespace Opyum.AudioBase
{
    public abstract class BaseContent : IContent
    {
        /// <summary>
        /// The location of the audio.
        /// </summary>
        public string Path { get; protected set; }
        /// <summary>
        /// The stream of the audio.
        /// </summary>
        public Stream AudioStream { get; protected set; }
        /// <summary>
        /// The <see cref="PlaylistItem"/> content type
        /// </summary>
        public ItemType ItemType { get; protected set; }
        /// <summary>
        /// The audio file type extention.
        /// </summary>
        public string FileType { get; set; }
    }
}
