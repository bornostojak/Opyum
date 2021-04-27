namespace Opyum.Structures.Playlist
{
    public enum TimeType
    {
        /// <summary>
        /// Time is dynamically assigned depending on the prior item.
        /// </summary>
        Dynamic = 0,
        /// <summary>
        /// The time is set and the item itself will position itself to be as close to the selected time as possible.
        /// <para>A leeway frame can be set for the item to arrange itself to if the designated time cannot be acuratly set.</para>
        /// </summary>
        Set = 1
    }
}
