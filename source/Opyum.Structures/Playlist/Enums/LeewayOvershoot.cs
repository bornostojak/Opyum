namespace Opyum.Structures.Playlist
{
    public enum LeewayOvershoot
    {
        /// <summary>
        /// The item will be set to the closest position based on the item start time and leeway.
        /// </summary>
        ClosestToLeeway = 0,
        /// <summary>
        /// Some other <see cref="PlaylistItem"/> will be adjusted to correctly place this item.
        /// </summary>
        ReplaceLinkedItem = 1,
        /// <summary>
        /// The linked <see cref="PlaylistItem"/> will be deleted so this item can be placed correctly in the list.
        /// </summary>
        DeleteLinkedItem = 2,
        /// <summary>
        /// The linked <see cref="PlaylistItem"/> will be deleted  or replaced so this item can be placed correctly in the list.
        /// </summary>
        ReplaceOrDeleteLinkedItem = 3,
        /// <summary>
        /// Will check if a jingle is set and:
        /// <para>1.) if a JINGLE is set then fade out the playling song, play JINGLE then play the Item,</para>
        /// <para>2.) if a JINGLE isn't set then fade out the playling song and play the Item. </para>
        /// </summary>
        Fixed = 4
    }
}
