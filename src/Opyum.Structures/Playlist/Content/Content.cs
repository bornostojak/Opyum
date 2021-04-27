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
        public ContentStatus Status { get; internal protected set; }

        /// <summary>
        /// The location of the audio (current).
        /// </summary>
        public virtual string Source { get; protected internal set; }






        /// <summary>
        /// Event raised by state change.
        /// </summary>
        public virtual event EventHandler StateChanged;

        /// <summary>
        /// Triggered on content change
        /// </summary>
        public virtual event EventHandler ContentChanged;



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
                this.Info = null;
                this.ContentChanged = null;
                this.Source = null;
                this.StateChanged = null;
                this.VolumeCurve = null;
            }
        }


        #endregion
    }
}
