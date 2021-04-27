using System;
using Opyum.Structures.Playlist;

namespace Opyum.Structures.PlaylistSupport
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
    public class ItemSupporter
    {
        protected string Path { get; set; }

        protected Content Content { get; set; }

        


        public ItemSupporter(string path)
        {
            Path = path;
            Content = (Content)InstantiateContentFromContentType(DeterminContentTypeFromPath());
        }

        //public void InitializeItem()
        //{
        //    //Check if the file exists
        //    if (!System.IO.File.Exists(Path))
        //    {
        //        Content.Status = ContentStatus.UnsuccessfulPathResolution;
        //        return;
        //    }

        //    //needs immeditate change
        //    var contentResolver = new ContentInterpreter(Path);
        //    if (contentResolver == null)
        //    {
        //        Content.Status = ContentStatus.UnsupportedFormat;
        //    }

        //    //Add content resolver so the audio can be played
        //    Content.AddContentResolver(contentResolver);

        //    //Update status
        //    Content.Status = ContentStatus.Preloading;

        //    //send caching request 

        //    SetProperties();

        //    PlaylistItem item = PlaylistItem.CreateFromContent(Content);

        //    ItemSetup(item);
            
        //}

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
        protected void ItemSetup(IPlaylistItem item)
        {
            //item.Content.Beginning = TimeSpan.Zero;
            //item.Content.DurationType = 0;
            //item.TimeType = 0;
            //item.Content.VolumeCurve = VolumeCurve.CreateDefault(item.Content.Duration);
            //item.Settings =
            //item.History = 
            //item.ItemType =

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
        protected void SetProperties()
        {
            
        }

        /// <summary>
        /// Unfinished.
        /// </summary>
        /// <returns></returns>
        protected SourceType DeterminContentTypeFromPath()
        {
            return SourceType.None;
        }

        //protected IContent InstantiateContentFromContentType(SourceType type) => FileContent.Create(Path);// <-------------------    CHANGE ASAP    -------------------> //
        protected IContent InstantiateContentFromContentType(SourceType type) => throw new NotImplementedException();
    }
}
