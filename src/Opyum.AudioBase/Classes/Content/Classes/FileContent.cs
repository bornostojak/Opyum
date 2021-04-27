using System;
using System.IO;
using Opyum.Structures;
using NAudio.Wave;

namespace Opyum.AudioBase
{
    [Opyum.AudioBase.Attributes.Content(ItemType.File, "Audio file stream loaded into the memory.")]
    public class FileContent : BaseContent, IContent, IDisposable
    {
        /// <summary>
        /// The <see cref="IFileFromMemoryStream"/> that contains the full file loaded into the memory.
        /// </summary>
        public IFileFromMemoryStream FileMemoryStream { get; protected set; }
        /// <summary>
        /// Returns the <see cref="Stream"/> form of the <see cref="IFileFromMemoryStream"/>.
        /// </summary>
        public new Stream AudioStream { get => (Stream)FileMemoryStream; }
        /// <summary>
        /// The <see cref="PlaylistItem"/> type.
        /// </summary>
        public new ItemType ItemType { get => ItemType.File; }
        /// <summary>
        /// The file path of the audio from <see cref="IFileFromMemoryStream"/> loaded into the memory.
        /// </summary>
        public new string Path => FileMemoryStream.FilePath;
        /// <summary>
        /// Returns the <see cref="WaveFormat"/> from the file stream.   
        /// </summary>
        public WaveFormat FileWaveFormat { get => new WaveFormat(new BinaryReader(AudioStream)); }


        protected FileContent()
        {

        }

        ~FileContent()
        {
            Dispose();
        }


        /// <summary>
        /// Creates a returns a new <see cref="FileContent"/> 
        /// </summary>
        /// <param name="file">The full path</param>
        /// <returns></returns>
        public static FileContent Create(string file) => new FileContent() { FileMemoryStream = FileFromMemoryStream.Create(file) };
        



        #region Garbage_Collection

        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }

        #endregion
    }
}
