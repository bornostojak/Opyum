using System;
using Opyum.Structures;

namespace Opyum.Structures.Playlist
{
    public interface IPlaylistItem : IDisposable
    {
        /// <summary>
        /// The content to play from.
        /// </summary>
        IContent Content { get; }

        /// <summary>
        /// The type of the item.
        /// <para>Impropper assignment will throw an <see cref="InvalidOperationException"/></para>
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        PlaylistItemType ItemType { get; }

        /// <summary>
        /// The time the audio is going to be played to start playing.
        /// </summary>
        DateTime PlayTime { get; set; }

        /// <summary>
        /// The time the audio is supposed to start playing.
        /// </summary>
        DateTime SetTime { get; }

        /// <summary>
        /// The status of the item (wheather it can be used, if it's playing...).
        /// </summary>
        ItemStatus Status { get; }

        /// <summary>
        /// Temporary tags that can be set in the running list.
        /// <para>These tags are applicable to only one item and are deleted the moment the item stops playling.</para>
        /// </summary>
        ITags Tags { get; set; }

        /// <summary>
        /// The type of item start time duration (SET or DYNAMIC)
        /// </summary>
        TimeType TimeType { get; }

        /// <summary>
        /// Used to generate the content.
        /// <para>Depending on the <paramref name="data"/>, the method will call the <see cref="ContentCreator"/>, which will determin if the data is a querry, path or stream location.</para>
        /// </summary>
        /// <param name="data"></param>
        void Generate(string data);

        /// <summary>
        /// Run tihis to initialize the content.
        /// <para>This will create a stream from the file source, or a local cache and, if set, will load file into memory.</para>
        /// </summary>
        void Initialize();

        event PlaylistItemChangedEventHandler Changed;
        event PlaylistItemChangedEventHandler NextItemUpdateRequest;
        event PlaylistItemChangedEventHandler Deletion;
    }
}