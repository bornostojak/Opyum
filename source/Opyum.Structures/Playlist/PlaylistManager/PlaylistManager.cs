namespace Opyum.Structures.Playlist
{
    public class PlaylistManager
    {
        protected PlaylistItem PreviousItem { get; private set; }
        protected PlaylistItem CurrentItem { get; private set; }
        protected PlaylistItem NextItem { get; private set; }


        protected PlaylistRearranger Rearranger { get; private set; } = PlaylistRearranger.Create();
    }
}
