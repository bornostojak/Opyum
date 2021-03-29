namespace Opyum.Structures.Playlist
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
        Item = 2,
        /// <summary>
        /// Audio slot. 
        /// <para>Might be empty or waiting for the requested audio.</para>
        /// </summary>
        Slot = 4,
        /// <summary>
        /// The beginning of a zone.
        /// </summary>
        //ZoneStart = 8,
        /// <summary>
        /// The end of a zone.
        /// </summary>
        //ZoneEnd = 9,
        /// <summary>
        /// Used to differentiate special commecial audio files from songs, jingles etc.
        /// </summary>
        Comemrcial = 8,
        /// <summary>
        /// A type of file used for raadio denotation, station identification or transitions between songs.
        /// </summary>
        Jingle = 16,
        /// <summary>
        /// The beginning of a <see cref="PlaylistZone"/>.
        /// </summary>
        Zone = 32,
        /// <summary>
        /// The item is a <see cref="PlaylistBlock"/>
        /// </summary>
        Block = 64

    }
}
