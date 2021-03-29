using System;
using System.IO;
using Opyum.Structures.Global;
using NAudio.Wave;

namespace Opyum.Structures.Playlist
{
    public interface IContent : IDisposable
    {

        /// <summary>
        /// The <see cref="PlaylistItem"/> content type
        /// </summary>
        SourceType SourceType { get; }

        /// <summary>
        /// The state of the content.
        /// </summary>
        ContentStatus Status { get; }

        /// <summary>
        /// The curve determening the volume of the audio while playing.
        /// </summary>
        VolumeCurve VolumeCurve { get; set; }

        /// <summary>
        /// Audio file information
        /// </summary>
        ContentInfo Info { get; }

        /// <summary>
        /// The raw data stream of the audio file.
        /// </summary>
        Stream Stream { get; }

        /// <summary>
        /// The location of the audio (current).
        /// </summary>
        string Source { get; }

        /// <summary>
        /// Used to initialize the content.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Used to log the events form the <see cref="IContent"/> into 
        /// the log file specified in the settings, or the default location.
        /// </summary>
        Logger Logger { get; }


        /// <summary>
        /// Event raised by state change.
        /// </summary>
        event EventHandler StateChanged;

        /// <summary>
        /// Triggered on content change
        /// </summary>
        event EventHandler ContentChanged;


        #region Unnecessary

        ///// <summary>
        ///// The object tasked with turning the raw audio data into audio samples & additional funcitionality.
        ///// </summary>
        //ContentInterpreter ContentInterpreter { get; }

        ///// <summary>
        ///// The point in the song where the audio starts.
        ///// <para>This should be ignored if the <see cref="IContent"/> is a <see cref="System.IO.Stream"/>.</para>
        ///// </summary>
        //TimeSpan Beginning { get; set; }

        ///// <summary>
        ///// The duration of the item.
        ///// <para>(for a <see cref="System.IO.Stream"/> it's set to 0.)</para>
        ///// </summary>
        //TimeSpan Duration { get; set; }

        ///// <summary>
        ///// The type of item duration (SET or DYNAMIC)
        ///// </summary>
        //DurationType DurationType { get; } 
        #endregion
    }
}