using System;

namespace Opyum.Structures.Playlist
{
    /// <summary>
    /// Unfinished
    /// <para>Unfinished</para>
    /// <para>Unfinished</para>
    /// <para>Unfinished</para>
    /// <para>Unfinished</para>
    /// <para>Unfinished</para>
    /// <para>Unfinished</para>
    /// <para>Unfinished</para>
    /// <para>Unfinished</para>
    /// <para>Unfinished</para>
    /// <para>Unfinished</para>
    /// <para>Unfinished</para>
    /// 
    /// </summary>
    public class VolumeCurve
    {
        public TimeSpan Beginning { get; internal protected set; }

        public TimeSpan Duration { get; internal protected set; }

        public TimeSpan MaxDuration { get; internal protected set; }

        FunctionGeneratedVolumeCurve FunctionCurve { get; set; }

        protected VolumeCurve(TimeSpan maxDuration)
        {
            MaxDuration = maxDuration;
            Duration = maxDuration;

        }

        public void Initialize()
        {

        }

        /// <summary>
        /// Unfinished
        /// <para>Unfinished</para>
        /// <para>Unfinished</para>
        /// <para>Unfinished</para>
        /// <para>Unfinished</para>
        /// <para>Unfinished</para>
        /// <para>Unfinished</para>
        /// <para>Unfinished</para>
        /// <para>Unfinished</para>
        /// <para>Unfinished</para>
        /// <para>Unfinished</para>
        /// <para>Unfinished</para>
        /// 
        /// </summary>
        internal static VolumeCurve CreateDefault(TimeSpan maxDuration) => new VolumeCurve(maxDuration);

    }
}
