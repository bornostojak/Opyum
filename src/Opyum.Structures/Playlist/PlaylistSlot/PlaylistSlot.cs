namespace Opyum.Structures.Playlist
{
    /// <summary>
    /// After setup, the slot waits for the proper file or source to load and plays it at the correct point in the playlist.
    /// </summary>
    [Opyum.Structures.Attributes.OpyumPlaylistItem]
    public class PlaylistSlot : PlaylistItem
    {
        public PlaylistSlot() : base()
        {

        }
        public override PlaylistItemType ItemType => PlaylistItemType.Slot;
    }
}