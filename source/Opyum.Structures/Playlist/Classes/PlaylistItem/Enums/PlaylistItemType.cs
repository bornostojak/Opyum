﻿namespace Opyum.Playlist
{
    public enum  PlaylistItemType
    {
        /// <summary>
        /// Item is not loaded or empty.
        /// </summary>
        None = 0,
        /// <summary>
        /// Audio itme. 
        /// <para>May be a file, a stream etc.</para>
        /// </summary>
        Item = 1,
        /// <summary>
        /// Audio slot. 
        /// <para>Might be empty or waiting for the requested audio.</para>
        /// </summary>
        Slot = 2,
        /// <summary>
        /// The beginning of a zone.
        /// </summary>
        ZoneStart = 4,
        /// <summary>
        /// The end of a zone.
        /// </summary>
        ZoneEnd = 8,
        /// <summary>
        /// Used to differentiate special commecial audio files from songs, jingles etc.
        /// </summary>
        Comemrcial = 16,
        /// <summary>
        /// A type of file used for raadio denotation, station identification or transitions between songs.
        /// </summary>
        Jingle = 32
    }
}