using NAudio.Wave;
using Opyum.Structures.Global;
using System;
using System.IO;

namespace Opyum.Structures.Playlist
{
    public class Content : IContent
    {
        internal Content()
        {

        }

        internal Content(TimeSpan beginning, TimeSpan duration, DurationType dType, VolumeCurve curve, ContentInfo info, SourceType sType, Stream stream, string source, ContentInterpreter interpreter, Logger logger) : this()
        {

            DurationType = dType;
            VolumeCurve = curve;
            Info = info;
            SourceType = sType;
            Stream = stream;
            Source = source;
            ContentInterpreter = interpreter;
            Logger = logger;
        }



        /// <summary>
        /// The type of item duration (SET or DYNAMIC)
        /// </summary>
        public virtual DurationType DurationType { get; protected internal set; }

        /// <summary>
        /// The curve determening the volume of the audio while playing.
        /// </summary>
        virtual public VolumeCurve VolumeCurve { get; set; }

        /// <summary>
        /// Audio file information
        /// </summary>
        public virtual ContentInfo Info { get; internal protected set; }

        /// <summary>
        /// The <see cref="PlaylistItem"/> content type
        /// </summary>
        public virtual SourceType SourceType { get; internal protected set; }

        /// <summary>
        /// The state of the content.
        /// </summary>
        public ContentStatus Status => ContentInterpreter == null ? _status : ContentInterpreter.Status;
        protected ContentStatus _status = 0;

        /// <summary>
        /// The raw data stream of the audio.
        /// </summary>
        public virtual Stream Stream { get; protected internal set; }

        /// <summary>
        /// The location of the audio (current).
        /// </summary>
        public virtual string Source { get; protected internal set; }



        /// <summary>
        /// The object tasked with turning the raw audio data into audio samples & additional funcitionality.
        /// </summary>
        public virtual ContentInterpreter ContentInterpreter { get; internal protected set; }


        /// <summary>
        /// Used to log the events form the <see cref="IContent"/> into 
        /// the log file specified in the settings, or the default location.
        /// </summary>
        public Logger Logger { get; protected internal set; }



        /// <summary>
        /// Event raised by state change.
        /// </summary>
        public virtual event EventHandler StateChanged;

        /// <summary>
        /// Triggered on content change
        /// </summary>
        public virtual event EventHandler ContentChanged;


        /// <summary>
        /// Used to initialize the content.
        /// </summary>
        public void Initialize()
        {
            
        }


        #region Garbage collection

        public virtual void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }

        private void Dispose(bool v)
        {
            if (v)
            {
                this.ContentInterpreter = null;
                _status = ContentStatus.Disposing;
                this.Info = null;
                this.ContentChanged = null;
                this.Source = null;
                this.StateChanged = null;
                this.Stream = null;
                this.VolumeCurve = null;
                this._status = ContentStatus.Disposed;
            }
        }


        #endregion
    }
}
