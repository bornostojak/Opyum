using Opyum.Structures.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.Structures.Playlist
{
    [Opyum.Structures.Attributes.OpyumContent(SourceType.File, "Audio file stream loaded into the memory.")]
    public class FileContent : IContent, IDisposable
    {



        private FileContent()
        {

        }

        /// <summary>
        /// The duration of the item.
        /// <para>(for a <see cref="System.IO.Stream"/> it's set to 0.)</para>
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// The type of item duration (SET or DYNAMIC)
        /// </summary>
        public DurationType DurationType { get; set; }

        /// <summary>
        /// The point in the song where the audio starts.
        /// <para>This should be ignored if the <see cref="IContent"/> is a <see cref="System.IO.Stream"/>.</para>
        /// </summary>
        public TimeSpan Beginning { get; set; }

        /// <summary>
        /// The curve determening the volume of the audio while playing.
        /// </summary>
        public VolumeCurve VolumeCurve { get; set; }


        /// <summary>
        /// The object tasked with turning the raw audio data into audio samples & additional funcitionality.
        /// </summary>
        public ContentInterpreter ContentInterpreter { get; protected internal set; }
        
        /// <summary>
        /// Audio file information
        /// </summary>
        public ContentInfo Info { get; protected internal set; }

        /// <summary>
        /// The <see cref="PlaylistItem"/> content type
        /// </summary>
        public SourceType SourceType { get; protected internal set; }

        /// <summary>
        /// The state of the content.
        /// </summary>
        public ContentStatus Status => ContentInterpreter.Status; 

        /// <summary>
        /// The raw data stream of the audio file.
        /// </summary>
        public Stream Stream { get; }


        /// <summary>
        /// The location of the audio (current).
        /// </summary>
        public string Source { get; protected internal set; }

        public Logger Logger => throw new NotImplementedException();

        /// <summary>
        /// The location of the audio (original).
        /// </summary>
        //string OriginalSourcePath { get; }

        /// <summary>
        /// Adds a sample converter to the Content.
        /// </summary>
        /// <param name="converter"></param>
        public void AddContentInterpreter(ContentInterpreter interpreter)
        {
            ContentInterpreter = interpreter;
        }

        /// <summary>
        /// Event raised by state change.
        /// </summary>
        public event EventHandler StateChanged;

        /// <summary>
        /// Triggered on content change
        /// </summary>
        public event EventHandler ContentChanged;


        private void CallStateChangedEvent(object sender, EventArgs e)
        {
            StateChanged?.Invoke(this, new EventArgs());
        }

        private void CallContentChangedEvent(object sender, EventArgs e)
        {
            ContentChanged?.Invoke(this, new EventArgs());
        }

        #region Garbage Collection

        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }

        private void Dispose(bool v)
        {
            if (v)
            {
                //dispose of unwanted shit
            }
        }


        #endregion
        public void Initialize()
        {
            throw new NotImplementedException();
        }

    }
}
