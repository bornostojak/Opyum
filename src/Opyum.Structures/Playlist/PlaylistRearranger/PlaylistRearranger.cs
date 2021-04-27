using System.Collections.Generic;

namespace Opyum.Structures.Playlist
{
    public class PlaylistRearranger
    {
        public virtual void RearrangeList(object list)
        {

        }

        public virtual void RearrangeItem(PlaylistItem item, object newPosition)
        {

        }

        public virtual void RearrangeItem(PlaylistItem item, PlaylistItem newPosition)
        {

        }

        public virtual void RearrangeItem(List<PlaylistItem> items, PlaylistItem newPosition)
        {

        }


        public static PlaylistRearranger Create() => new PlaylistRearranger();
    }
}
