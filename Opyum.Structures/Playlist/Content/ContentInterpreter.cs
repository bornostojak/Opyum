using System;
using System.Collections.Generic;

namespace Opyum.Structures.Playlist
{
    /// <summary>
    /// Resolves requests for contents.
    /// <para>If the content is a file path, then all the data will be loaded from the file info (and passed through to the file manager for redundancy control)</para>
    /// <para>If the content is a db querry, all the data will be loaded from the db.</para>
    /// <para>If the content is s stream, the data will be loaded from the stream (if present).</para>
    /// </summary>
    public class ContentInterpreter : IDisposable
    {
        /// <summary>
        /// The status of the interpreter.
        /// </summary>
        public ContentStatus Status { get; protected internal set; }



        protected ContentInterpreter()
        {

        }

        ~ContentInterpreter()
        {

        }


        public ContentInterpreter(string path, Content content) : this()
        {
            
        }

        /// <summary>
        /// <para>UNFINSHED</para>
        /// <para>UNFINSHED</para>
        /// <para>UNFINSHED</para>
        /// <para>UNFINSHED</para>
        /// Sets up the <see cref="ContentInterpreter"/> so that it can track wheather the file changes.
        /// </summary>
        /// <param name="source"></param>
        public void Setup(string source)
        {

        }

        public void Load()
        {

        }



        /// <summary>
        /// UNFINISHED
        /// UNFINISHED
        /// UNFINISHED
        /// </summary>
        /// <returns></returns>
        public static ContentInterpreter AssignContentInterpreter()
        {
            return new ContentInterpreter();
        }

        #region GarbageCollection

        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }

        #endregion


    }
}
