namespace Opyum.Structures.Playlist
{
    /// <summary>
    /// A <see cref="PlaylistZone"/> is a <see cref="PlaylistItem"/> containing item rules that can be applied to a specific time span in the 
    /// <see cref="Playlist"/>, or from a specific <see cref="PlaylistItem"/>, determined by a certain number of consecuitve <see cref="PlaylistItem"/>s,
    /// or up until a certain linked <see cref="PlaylistItem"/>.
    /// 
    /// <para>
    /// If a rule determines that an item is not allowed to be played inside the <see cref="PlaylistZone"/>'s area of effect,
    /// the item will be skipped, and the notion will be evidented in the <see cref="PlaylistItemHistory"/>, marked as appropriated by the server. 
    /// This form of transgression can be further notified to the appropriate individual by program or mail.
    /// Furthermore, once the <see cref="Playlist"/> is loaded into the program, a checking procedure will automatically check
    /// for conflicts inside of <see cref="PlaylistZone"/>s, which will be immediately shown as a warning on the control screen.
    /// Further rules can be apointed, inside of the server, to invoke additional measures of notifying individuals of misplaced items in zones.
    /// </para>
    /// 
    /// <para>
    /// A zone can be used to limit access to that part of the list for a given item group(s) in order to control allowed content,
    /// or prevent unwanted content from being played during that part of the list.
    /// </para>
    /// 
    /// <para>
    /// A <see cref="PlaylistZone"/> can, but doesn't have to, override the <see cref="Playlist"/>'s rules, it can add additional rules
    /// to that part of the <see cref="Playlist"/> or have a copletely independant rule set in regards to the <see cref="Playlist"/>.
    /// </para>
    /// </summary>
    [Opyum.Structures.Attributes.OpyumPlaylistItem]
    public class PlaylistZone : PlaylistItem
    {
        public PlaylistZone() : base()
        {

        }
    }
}
