using System.IO;

namespace Opyum.AudioBase
{
    public interface IContent
    {
        /// <summary>
        /// The stream of the audio.
        /// </summary>
        Stream AudioStream { get; }
        /// <summary>
        /// The <see cref="PlaylistItem"/> content type
        /// </summary>
        ItemType ItemType { get; }
        /// <summary>
        /// The location of the audio.
        /// </summary>
        string Path { get; }
        /// <summary>
        /// The audio file type extention.
        /// </summary>
        string FileType { get; }
    }
}